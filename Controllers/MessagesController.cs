using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Loop.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessagesController : ControllerBase
    {
        // Ping simple: devuelve timestamps del servidor (ISO 8601 UTC)
        [HttpPost("ping")]
        public IActionResult Ping([FromBody] JsonElement body)
        {
            // Timestamp de recepción en servidor (UTC ISO)
            var serverReceive = DateTime.UtcNow.ToString("o");

            // (Si quieres simular procesamiento, hazlo aquí; por simplicidad no lo hacemos)
            var serverSend = DateTime.UtcNow.ToString("o");

            return Ok(new {
                server_receive_ts = serverReceive,
                server_send_ts = serverSend
            });
        }
    }
}