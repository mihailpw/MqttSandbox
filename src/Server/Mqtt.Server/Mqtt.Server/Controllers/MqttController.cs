using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mqtt.Server.Trash;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Subscribing;
using MQTTnet.Client.Unsubscribing;

namespace Mqtt.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MqttController : ControllerBase
    {
        [HttpGet("s")]
        public async Task<IActionResult> Subscribe()
        {
            var mqttClient = await MqttClientContext.GetConnectedClientAsync();
            var subscribeResult = await mqttClient.SubscribeAsync(
                new MqttClientSubscribeOptionsBuilder()
                   .WithTopicFilter("room/switcher/on-off")
                   .Build(),
                CancellationToken.None);

            return Ok();
        }

        [HttpGet("u")]
        public async Task<IActionResult> Unsubscribe()
        {
            var mqttClient = await MqttClientContext.GetConnectedClientAsync();
            var unsubscribeResult = await mqttClient.UnsubscribeAsync(
                new MqttClientUnsubscribeOptions
                {
                    TopicFilters = new List<string> { "room/switcher/on-off" }
                },
                CancellationToken.None);

            return Ok();
        }

        [HttpGet("p")]
        public async Task<IActionResult> Publish()
        {
            var mqttClient = await MqttClientContext.GetConnectedClientAsync();
            var publishResult = await mqttClient.PublishAsync(
                new MqttApplicationMessageBuilder()
                   .WithTopic("room/switcher/on-off")
                   .WithResponseTopic("")
                   .WithPayload("payload")
                   .Build(),
                CancellationToken.None);

            
            return Ok();
        }
    }
}
