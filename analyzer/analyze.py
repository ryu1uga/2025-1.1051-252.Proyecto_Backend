import time, os, json
import numpy as np

LOG_PATH = os.getenv("LOG_PATH", "/data/probe_events.log")
WINDOW = int(os.getenv("WINDOW", "100"))
Z_THRESH = float(os.getenv("Z_THRESH", "4.0"))

def tail(path):
    with open(path, "r", encoding="utf-8") as f:
        f.seek(0,2)
        while True:
            line = f.readline()
            if not line:
                time.sleep(0.5)
                continue
            yield line

def parse_event(line):
    try:
        return json.loads(line)
    except:
        return None

def main():
    window = []
    print("Analyzer started, watching:", LOG_PATH)
    while not os.path.exists(LOG_PATH):
        print("Waiting for log:", LOG_PATH)
        time.sleep(0.5)
    for line in tail(LOG_PATH):
        ev = parse_event(line)
        if not ev:
            continue
        rtt = ev.get("rtt")
        if rtt is None:
            print("[ERR]", ev.get("error"), ev.get("ts"))
            continue
        window.append(rtt)
        if len(window) > WINDOW:
            window.pop(0)
        arr = np.array(window)
        mu = arr.mean()
        sigma = arr.std(ddof=0) if arr.size>1 else 0.0
        z = (rtt - mu) / (sigma + 1e-9)
        if z > Z_THRESH:
            print(f"[ALERTA] {ev.get('ts')} probe={ev.get('probe_id')} rtt={rtt:.3f}s mu={mu:.3f}s sigma={sigma:.3f}s z={z:.2f}")
        else:
            print(f"[OK] {ev.get('ts')} rtt={rtt:.3f}s z={z:.2f}")

if __name__ == "__main__":
    main()