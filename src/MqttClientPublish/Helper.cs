using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using System.Text;

namespace MqttClientPublisher
{
    public static class Helper
    {
        public static void AddClientOptions(this IManagedMqttClient managedMqttClient)
        {
            Console.Write("CONNECTION: ");

            managedMqttClient.ConnectedAsync += args =>
            {
                if (args.ConnectResult.ResultCode == MqttClientConnectResultCode.Success)
                {
                    Console.WriteLine($"Publisher connected successfully.");
                }
                else
                {
                    Console.WriteLine($"Publisher could not connect to broker.");
                }

                return Task.CompletedTask;
            };

            managedMqttClient.DisconnectedAsync += args =>
            {
                if (args.ClientWasConnected)
                {
                    Console.WriteLine($"Publisher was disconnected.");
                }
                else if (args.ConnectResult.ResultCode == MqttClientConnectResultCode.Success)
                {
                    Console.WriteLine($"Publisher disconnected successfully.");
                }
                else
                {
                    Console.WriteLine($"Publisher could not disconnect from broker.");
                }

                return Task.CompletedTask;
            };
        }

        public static void AddMessageOptions(this IManagedMqttClient managedMqttClient)
        {
            managedMqttClient.ApplicationMessageProcessedAsync += args =>
            {
                var message = Encoding.UTF8.GetString(args.ApplicationMessage.ApplicationMessage.Payload);
                var topic = args.ApplicationMessage.ApplicationMessage.Topic;

                Console.WriteLine($"PUBLISHED: A new message \"{message}\" was published for topic \"{topic}\".");
                Console.WriteLine($"QUEUE STATUS: Pending messages = {managedMqttClient.PendingApplicationMessagesCount}.");

                return Task.CompletedTask;
            };
        }
    }
}
