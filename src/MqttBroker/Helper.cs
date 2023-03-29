using MQTTnet.Internal;
using MQTTnet.Protocol;
using MQTTnet.Server;
using System.Text;

namespace MqttBroker
{
    public static class Helper
    {
        private static Dictionary<string, (string username, string password)> allowedConnections = new Dictionary<string, (string username, string password)>()
        {
            { "publisher#1", ("publisher", "/pass228") },
            { "subscriber#1", ("subscriber", "/pass228") }
        };

        public static async Task Start(this MqttServer mqttServer)
        {
            mqttServer.StartedAsync += args =>
            {
                Console.WriteLine("-----> The MQTT server was started. <-----\n");

                return Task.CompletedTask;
            };

            await mqttServer.StartAsync();
        }

        public static async Task Stop(this MqttServer mqttServer)
        {
            mqttServer.StoppedAsync += args =>
            {
                Console.WriteLine("-----> The MQTT server was stopped. <-----\n");

                return Task.CompletedTask;
            };

            await mqttServer.StopAsync();
        }

        public static void ValidateConnection(this MqttServer mqttServer)
        {
            mqttServer.ValidatingConnectionAsync += args =>
            {
                if (!allowedConnections.ContainsKey(args.ClientId))
                {
                    args.ReasonCode = MqttConnectReasonCode.ClientIdentifierNotValid;
                }

                if (!allowedConnections.Values.Any(x => x.username.Equals(args.UserName)))
                {
                    args.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                }

                if (!allowedConnections.Values.Any(x => x.password.Equals(args.Password)))
                {
                    args.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                }

                return Task.CompletedTask;
            };
        }

        public static void AddClientNotices(this MqttServer mqttServer)
        {
            mqttServer.ClientConnectedAsync += args =>
            {
                Console.WriteLine($"CONNECTED: Client({args.ClientId}) \"{args.UserName}\" connected to {args.Endpoint}.");
                return Task.CompletedTask;
            };

            mqttServer.ClientDisconnectedAsync += args =>
            {
                Console.WriteLine($"DISCONNECTED: Client({args.ClientId}) disconneted from {args.Endpoint}.");
                return Task.CompletedTask;
            };

            mqttServer.ClientSubscribedTopicAsync += args =>
            {
                Console.WriteLine($"SUBSCRIBED: Client({args.ClientId}) subscribed to topic {args.TopicFilter}.");
                return Task.CompletedTask;
            };

            mqttServer.ClientUnsubscribedTopicAsync += args =>
            {
                Console.WriteLine($"UNSUBSCRIBED: Client({args.ClientId}) unsubscribed from topic {args.TopicFilter}.");
                return Task.CompletedTask;
            };

            mqttServer.ClientAcknowledgedPublishPacketAsync += args =>
            {
                Console.WriteLine($"ACKNOWLEDGED: Client({args.ClientId}) was acknowledged of a new published packet for topic {args.PublishPacket.Topic}.");
                return Task.CompletedTask;
            };
        }

        public static void AddMessageInterception(this MqttServer mqttServer)
        {
            mqttServer.InterceptingPublishAsync += args =>
            {
                var payload = args.ApplicationMessage.Payload;
                var topic = args.ApplicationMessage.Topic;
                Console.WriteLine($"NEW MESSAGE PUBLISH: Publisher({args.ClientId}) published a message \"{Encoding.UTF8.GetString(payload)}\" for topic \"{topic}\".");

                return CompletedTask.Instance;
            };

            mqttServer.InterceptingSubscriptionAsync += args =>
            {
                Console.WriteLine("InterceptingSubscriptionAsync");

                return CompletedTask.Instance;
            };

            mqttServer.InterceptingUnsubscriptionAsync += args =>
            {
                Console.WriteLine("InterceptingUnsubscriptionAsync");

                return CompletedTask.Instance;
            };
        }
    }
}
