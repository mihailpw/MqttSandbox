using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;

namespace Mqtt.Server.Common
{
    internal class MqttHandler
    {
        private readonly IMqttServer _mqttServer;

        private readonly List<string> _connectedClients = new List<string>();

        public MqttHandler(IMqttServer mqttServer)
        {
            _mqttServer = mqttServer;
            _mqttServer.Options.ClientId = "server";
        }

        public Task ClientConnectedAsync(MqttServerClientConnectedEventArgs eventArgs)
        {
            _connectedClients.Add(eventArgs.ClientId);
            return Task.CompletedTask;
        }

        public Task ClientDisconnectedAsync(MqttServerClientDisconnectedEventArgs eventArgs)
        {
            _connectedClients.Remove(eventArgs.ClientId);
            return Task.CompletedTask;
        }

        public Task ClientSubscribedAsync(MqttServerClientSubscribedTopicEventArgs eventArgs)
        {
            return Task.CompletedTask;
        }

        public Task ClientUnsubscribedAsync(MqttServerClientUnsubscribedTopicEventArgs eventArgs)
        {
            return Task.CompletedTask;
        }

        public async Task MessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            Console.WriteLine(MqttUtils.ToString("server", eventArgs.ApplicationMessage));
            if (eventArgs.ClientId == null)
            {
                return;
            }
            
            await _mqttServer.PublishAsync(b => b
               .WithTopic("topic")
               .WithPayload("payload")
               .WithContentType("text/json")
               .WithSubscriptionIdentifier(1)
               .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtMostOnce));
            
            await _mqttServer.PublishAsync(b => b
               .WithTopic("topic")
               .WithPayload("payload")
               .WithContentType("text/json"));
        }

        public Task StartedAsync()
        {
            return Task.CompletedTask;
        }

        public Task StoppedAsync()
        {
            return Task.CompletedTask;
        }
    }
}