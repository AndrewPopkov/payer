using System.Net;



namespace serverpayer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Создаем
            Server server = new Server();
            // Запускаем
            server.start(IPAddress.Parse("127.0.0.1"), 81, 10, "");
            // останавливаем
            //server.stop();
        }
    }
}
