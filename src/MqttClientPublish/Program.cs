namespace MqttClientPublisher
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Client client = new Client();
            await client.StartClient();
        }
    }
}