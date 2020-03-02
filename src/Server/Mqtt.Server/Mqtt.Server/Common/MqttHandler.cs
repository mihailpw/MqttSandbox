using System.Threading.Tasks;
using MQTTnet.Server;

namespace Mqtt.Server.Common
{
    internal class MqttHandler
    {
        public Task ClientConnectedAsync(MqttServerClientConnectedEventArgs eventArgs)
        {
            return Task.CompletedTask;
        }

        public Task ClientDisconnectedAsync(MqttServerClientDisconnectedEventArgs eventArgs)
        {
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