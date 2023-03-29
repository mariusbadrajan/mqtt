namespace MqttBroker
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Broker broker = new Broker(false);
            await broker.RunServer();
        }
    }
}