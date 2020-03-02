using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Mqtt.Server.Common;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;

namespace Mqtt.Server.Trash
{
    internal static class MqttClientContext
    {
        private static IMqttClient Client { get; set; }

        public static async Task<IMqttClient> GetConnectedClientAsync()
        {
            if (Client == null)
            {
                Client = new MqttFactory().CreateMqttClient();
                Client.ConnectedHandler = new MqttClientConnectedHandlerDelegate(e => { });
                Client.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(e => { });
                Client.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(e =>
                {
                    Console.WriteLine(MqttUtils.ToString("client", e.ApplicationMessage));
                });
            }

            if (!Client.IsConnected)
            {
                var options = new MqttClientOptionsBuilder()
                   .WithTcpServer("127.0.0.1", 61613)
                   .WithCredentials("admin", "password")
                   .Build();
                var authenticateResult = await Client.ConnectAsync(options, CancellationToken.None);
                if (authenticateResult.ResultCode != MqttClientConnectResultCode.Success)
                {
                    throw new Exception($"Auth failed. Status={authenticateResult.ResultCode}");
                }

            }

            return Client;
        }
    }
}