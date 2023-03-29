using MQTTnet;
using MQTTnet.Server;

namespace MqttBroker
{
    internal class Broker
    {
        private readonly MqttFactory MqttFactory;

        private const int ServerPort = 1337;

        public Broker(bool withDefaultLogger = true)
        {
            MqttFactory = withDefaultLogger ? new MqttFactory(new ConsoleLogger()) : new MqttFactory();
        }

        private MqttServerOptions BuildServerOptions()
        {
            MqttServerOptions mqttServerOptions = new MqttServerOptionsBuilder()
                                                        .WithDefaultEndpoint()
                                                        .WithDefaultEndpointPort(ServerPort)
                                                        .Build();

            return mqttServerOptions;
        }

        public async Task RunServer()
        {
            MqttServerOptions mqttServerOptions = BuildServerOptions();
            string? consoleInput = "";

            using MqttServer mqttServer = MqttFactory.CreateMqttServer(mqttServerOptions);

            mqttServer.ValidateConnection();
            mqttServer.AddClientNotices();
            mqttServer.AddMessageInterception();

            await mqttServer.Start();

            Console.WriteLine("---- Enter \"STOP\" to stop the server. ----\n");
            while (!consoleInput.Equals("STOP"))
            {
                consoleInput = Console.ReadLine();
            }

            await mqttServer.Stop();
        }
    }
}
