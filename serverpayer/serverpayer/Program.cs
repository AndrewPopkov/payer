using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using serverHttp;
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
            server.start(IPAddress.Parse("127.0.0.1"), 80, 10, contentpath);
            // останавливаем
            //server.stop();
        }
    }
}
