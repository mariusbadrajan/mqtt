using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;
using System.Text;

namespace MqttClientSubscriber
{
    internal class Client
    {
        private readonly MqttFactory MqttFactory;

        private const string Server = "localhost";
        private const int ServerPort = 1337;

        private const string ClientId = "subscriber#1";
        private const string Username = "subscriber";
        private const string Password = "/pass228";

        public Client()
        {
            MqttFactory = new MqttFactory();
        }

        private ManagedMqttClientOptions BuildClientOptions()
        {
            MqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder()
                                                        .WithTcpServer(Server, ServerPort)
                                                        .WithClientId(ClientId)
                                                        .WithCredentials(Username, Encoding.ASCII.GetBytes(Password))
                                                        .Build();

            ManagedMqttClientOptions managedMqttClientOptions = new ManagedMqttClientOptionsBuilder()
                                                                        .WithClientOptions(mqttClientOptions)
                                                                        .Build();

            return managedMqttClientOptions;
        }

        public async Task StartClient()
        {
            ManagedMqttClientOptions managedMqttClientOptions = BuildClientOptions();

            using IManagedMqttClient managedMqttClient = MqttFactory.CreateManagedMqttClient();

            managedMqttClient.AddClientOptions();
            managedMqttClient.AddMessageOptions();

            await managedMqttClient.StartAsync(managedMqttClientOptions);

            await managedMqttClient.SubscribeAsync("Test topic");

            Console.ReadLine();
        }
    }
}
