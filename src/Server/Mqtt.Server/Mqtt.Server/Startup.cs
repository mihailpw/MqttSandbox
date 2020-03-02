using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mqtt.Server.Common;
using MQTTnet;
using MQTTnet.AspNetCore;
using MQTTnet.Protocol;
using MQTTnet.Server;

namespace Mqtt.Server
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<MqttHandler>();
            services.AddSingleton<MqttServer>();


            var hostIp = System.Net.IPAddress.Parse(_configuration["MqttOption:HostIp"]);
            var hostPort = int.Parse(_configuration["MqttOption:HostPort"]);
            var timeout = TimeSpan.FromMilliseconds(int.Parse(_configuration["MqttOption:Timeout"]));
            var username = _configuration["MqttOption:UserName"];
            var password = _configuration["MqttOption:Password"];

            var option = new MqttServerOptionsBuilder()
               .WithDefaultEndpointBoundIPAddress(hostIp)
               .WithDefaultEndpointPort(hostPort)
               .WithDefaultCommunicationTimeout(timeout)
               .WithConnectionValidator(
                    t => t.ReasonCode = t.Username == username && t.Password == password
                        ? MqttConnectReasonCode.Success
                        : MqttConnectReasonCode.BadUserNameOrPassword)
               .Build();
            services
               .AddHostedMqttServer(option)
               .AddMqttConnectionHandler()
               .AddConnections();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapConnectionHandler<MqttConnectionHandler>("/data",
                    options => options.WebSockets.SubProtocolSelector =
                        MQTTnet.AspNetCore.ApplicationBuilderExtensions.SelectSubProtocol);
            });
            app.UseMqttEndpoint("/data");
            app.UseMqttServer(server =>
            {
                var handler = provider.GetService<MqttHandler>();
                server.ClientConnectedHandler = new MqttServerClientConnectedHandlerDelegate(handler.ClientConnectedAsync);
                server.ClientDisconnectedHandler = new MqttServerClientDisconnectedHandlerDelegate(handler.ClientDisconnectedAsync);
                server.ClientSubscribedTopicHandler = new MqttServerClientSubscribedHandlerDelegate(handler.ClientSubscribedAsync);
                server.ClientUnsubscribedTopicHandler = new MqttServerClientUnsubscribedTopicHandlerDelegate(handler.ClientUnsubscribedAsync);
                server.StartedHandler = new MqttServerStartedHandlerDelegate(_ => handler.StartedAsync());
                server.StoppedHandler = new MqttServerStoppedHandlerDelegate(_ => handler.StoppedAsync());
            });
        }
    }
}
