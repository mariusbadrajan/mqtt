using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using System.Text;

namespace MqttClientPublisher
{
    internal class Client
    {
        private readonly MqttFactory MqttFactory;

        private const string Server = "localhost";
        private const int ServerPort = 1337;

        private const string ClientId = "publisher#1";
        private const string Username = "publisher";
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

            var message = "Test message";
            await managedMqttClient.EnqueueAsync("Test topic", message);

            SpinWait.SpinUntil(() => managedMqttClient.PendingApplicationMessagesCount == 0, -1);
            Console.ReadLine();
        }
    }
}
