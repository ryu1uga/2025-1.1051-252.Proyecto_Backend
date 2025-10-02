import time, os, json, requests
from datetime import datetime

TARGET = os.getenv("TARGET_URL", "http://web:80/api/messages/ping")
INTERVAL = float(os.getenv("INTERVAL", "1.0"))
LOG_PATH = os.getenv("LOG_PATH", "/data/probe_events.log")
PROBE_ID = os.getenv("PROBE_ID", "probe-1")
TIMEOUT = float(os.getenv("TIMEOUT", "5"))

def now_ts():
    return datetime.utcnow().isoformat() + "Z"

def write_event(ev):
    with open(LOG_PATH, "a", encoding="utf-8") as f:
        f.write(json.dumps(ev, default=str) + "\n")

def run():
    while True:
        c_send = time.time()
        try:
            payload = {"probe_id": PROBE_ID, "client_send_iso": now_ts()}
            resp = requests.post(TARGET, json=payload, timeout=TIMEOUT)
            c_recv = time.time()
            rtt = c_recv - c_send
            jr = None
            if resp is not None and resp.ok:
                try:
                    jr = resp.json()
                except:
                    jr = {}
            event = {
                "probe_id": PROBE_ID,
                "client_send_ts": c_send,
                "client_recv_ts": c_recv,
                "rtt": rtt,
                "server_receive_ts": jr.get("server_receive_ts") if jr else None,
                "server_send_ts": jr.get("server_send_ts") if jr else None,
                "status_code": resp.status_code if resp is not None else None,
                "ts": now_ts()
            }
            write_event(event)
        except Exception as e:
            c_recv = time.time()
            event = {
                "probe_id": PROBE_ID,
                "client_send_ts": c_send,
                "client_recv_ts": c_recv,
                "rtt": None,
                "error": str(e),
                "ts": now_ts()
            }
            write_event(event)
        time.sleep(INTERVAL)

if __name__ == "__main__":
    run()