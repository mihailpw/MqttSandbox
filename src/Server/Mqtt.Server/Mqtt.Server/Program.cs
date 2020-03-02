using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MQTTnet.AspNetCore;

namespace Mqtt.Server
{
    public class Program
    {
        private const int MqttPort = 61613;
        private const int ApiPort = 5001;

        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(c => c.ListenAnyIP(MqttPort, o => o.UseMqtt()));
                    webBuilder.UseKestrel(c => c.ListenAnyIP(ApiPort));
                    webBuilder.UseStartup<Startup>();
                })
               .Build()
               .Run();
        }
    }
}
