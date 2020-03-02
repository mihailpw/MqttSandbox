using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MQTTnet.Protocol;
using MQTTnet.Server;

namespace Mqtt.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MqttController : ControllerBase
    {
        private readonly MqttServer _mqttServer;

        public MqttController(MqttServer mqttServer)
        {
            _mqttServer = mqttServer;
        }

        [HttpGet("s")]
        public async Task<IActionResult> Subscribe()
        {
            await _mqttServer.SubscribeAsync("client", "t", MqttQualityOfServiceLevel.AtLeastOnce);
            return Ok();
        }

        [HttpGet("p")]
        public async Task<IActionResult> Publish()
        {
            await _mqttServer.PublishAsync(b => b.WithTopic("t").WithPayload("payload"));
            return Ok();
        }
    }
}
