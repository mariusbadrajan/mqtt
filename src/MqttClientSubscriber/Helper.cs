using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using System.Text;

namespace MqttClientSubscriber
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
                    Console.WriteLine($"Subscriber connected successfully.");
                }
                else
                {
                    Console.WriteLine($"Subscriber could not connect to broker.");
                }

                return Task.CompletedTask;
            };

            managedMqttClient.DisconnectedAsync += args =>
            {
                if (args.ClientWasConnected)
                {
                    Console.WriteLine($"Subscriber was disconnected.");
                }
                else if (args.ConnectResult.ResultCode == MqttClientConnectResultCode.Success)
                {
                    Console.WriteLine($"Subscriber disconnected successfully.");
                }
                else
                {
                    Console.WriteLine($"Subscriber could not disconnect from broker.");
                }

                return Task.CompletedTask;
            };
        }

        public static void AddMessageOptions(this IManagedMqttClient managedMqttClient)
        {
            managedMqttClient.ApplicationMessageReceivedAsync += args =>
            {
                var message = Encoding.UTF8.GetString(args.ApplicationMessage.Payload);
                var topic = args.ApplicationMessage.Topic;

                Console.WriteLine($"RECEIVED: A new message \"{message}\" was received for topic \"{topic}\".");

                return Task.CompletedTask;
            };
        }
    }
}
