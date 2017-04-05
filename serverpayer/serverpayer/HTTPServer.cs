using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Text.RegularExpressions;

namespace serverpayer
{
    class Server
    {
        public bool running = false; //Запущено ли?
        private int timeout = 8; // Лиммт времени на приём данных.
        private Encoding charEncoder = Encoding.UTF8; // Кодировка
        private Socket serverSocket; // Нащ сокет
        private string contentPath; // Корневая папка для контента

        private Dictionary<string, string> extensions = new Dictionary<string, string>()
        { 
            { "htm", "text/html" },
            { "html", "text/html" },
            { "xml", "text/xml" },
            { "txt", "text/plain" },
            { "css", "text/css" },
            { "png", "image/png" },
            { "gif", "image/gif" },
            { "jpg", "image/jpg" },
            { "jpeg", "image/jpeg" },
            { "zip", "application/zip"}
        };

        public bool start(IPAddress ipAddress, int port, int maxNOfCon, string contentPath)
        {
            if (running) return false; // Если уже запущено, то выходим

            try
            {
                // tcp/ip сокет (ipv4)
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(ipAddress, port));
                serverSocket.Listen(maxNOfCon);
                serverSocket.ReceiveTimeout = timeout;
                serverSocket.SendTimeout = timeout;
                running = true;
                this.contentPath = contentPath;
            }
            catch 
            { 
                return false; 
            }

            // Наш поток ждет новые подключения и создает новые потоки.
            Thread requestListenerT = new Thread(() =>
            {
                while (running)
                {
                    Socket clientSocket;
                    try
                    {
                        clientSocket = serverSocket.Accept();
                        // Создаем новый поток для нового клиента и продолжаем слушать сокет.
                        Thread requestHandler = new Thread(() =>
                        {
                            clientSocket.ReceiveTimeout = timeout;
                            clientSocket.SendTimeout = timeout;
                            try 
                            { 
                                handleTheRequest(clientSocket); 
                            }
                            catch (Exception ex)
                            {
                                try 
                                { 
                                    clientSocket.Close(); 
                                }
                                catch 
                                {

                                }
                            }
                        });
                        requestHandler.Start();
                    }
                    catch(Exception ex)
                    {
                        
                    }
                }
            });
            requestListenerT.Start();

            return true;
        }

        public void stop()
        {
            if (running)
            {
                running = false;
                try 
                { 
                    serverSocket.Close(); 
                }
                catch (Exception ex)
                {

                }
                serverSocket = null;
            }
        }

        private void handleTheRequest(Socket clientSocket)
        {
            string requestedFile;
            byte[] buffer = new byte[10240]; // 10 kb, just in case
            int receivedBCount = clientSocket.Receive(buffer); // Получаем запрос
            string strReceived = charEncoder.GetString(buffer, 0, receivedBCount);
            //Match ReqMatch = Regex.Match(strReceived, @"^\w+\s+([^\s\?]+)[^\s]*\s+HTTP/.*|");
            // Парсим запрос
            string httpHead = strReceived.Substring(0, strReceived.IndexOf("\r\n"));
            //"GET /api?order_id=41 HTTP/1.1"
            string httpMethod = httpHead.Substring(0, httpHead.IndexOf(" "));
            Regex re = new Regex(httpHead.Substring(0, httpHead.IndexOf(" ")+1), RegexOptions.IgnoreCase);
            string requestedUrl = re.Replace(httpHead, "");
            re = new Regex(@"( HTTP){1}.*$", RegexOptions.IgnoreCase);
            requestedUrl = re.Replace(requestedUrl, "");

            //httpHead.Replace(httpMethod, "");
            //int start = strReceived.IndexOf(httpMethod) + httpMethod.Length + 1;
            //int length = strReceived.LastIndexOf("HTTP") - start - 1;
            //string requestedUrl = strReceived.Substring(start, length);
            if(Regex.Match(requestedUrl,@"^(/api)").Success)
            {
                apifacade = new APIFacade(requestedUrl).ge;
            }
            switch (httpMethod)
            {
                case "GET":
                    requestedFile = requestedUrl.Split('?')[0];
                    break;

                case "POST":
                    requestedFile = requestedUrl.Split('?')[0];
                    break;

                case "PUT":

                    break;

                case "DELETE":


                    break;
              
                default:

                    notImplemented(clientSocket);

                    break;
            }

           

            if (httpMethod.Equals("GET") || httpMethod.Equals("POST"))
                requestedFile = requestedUrl.Split('?')[0];
            else // Вы можете реализовать другие методы
            {
                notImplemented(clientSocket);
                return;
            }
            //обработка запроса для нахождения файла  в файловом менеджере
            requestedFile = requestedFile.Replace("/", "\\").Replace("\\..", ""); // Not to go back
            //start = requestedFile.LastIndexOf('.') + 1;
            //if (start > 0)
            //{
            //    length = requestedFile.Length - start;
            //    string extension = requestedFile.Substring(start, length);
            //    if (extensions.ContainsKey(extension)) // Мы поддерживаем это расширение?
            //        if (File.Exists(contentPath + requestedFile)) // Если да
            //            // ТО отсылаем запрашиваемы контент:
            //            sendOkResponse(clientSocket, File.ReadAllBytes(contentPath + requestedFile), extensions[extension]);
            //        else
            //            notFound(clientSocket); // Мы не поддерживаем данный контент.
            //}
            //else
            //{
            //    // Если файл не указан, пробуем послать index.html
            //    // Вы можете добавить больше(например "default.html")
            //    if (requestedFile.Substring(length - 1, 1) != "\\")
            //        requestedFile += "\\";
            //    if (File.Exists(contentPath + requestedFile + "index.htm"))
            //        sendOkResponse(clientSocket, File.ReadAllBytes(contentPath + requestedFile + "\\index.htm"), "text/html");
            //    else if (File.Exists(contentPath + requestedFile + "index.html"))
            //        sendOkResponse(clientSocket, File.ReadAllBytes(contentPath + requestedFile + "\\index.html"), "text/html");
            //    else
            //        notFound(clientSocket);
            //}
        }

        private void notImplemented(Socket clientSocket)
        {
            sendResponse(clientSocket, "<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"></head><body><h2>Atasoy Simple Web Server</h2><div>501 - Method Not Implemented</div></body></html>", "501 Not Implemented", "text/html");
        }

        private void notFound(Socket clientSocket)
        {
            sendResponse(clientSocket, "<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"></head><body><h2>Atasoy Simple Web Server</h2><div>404 - Not Found</div></body></html>", "404 Not Found", "text/html");
        }

        private void sendOkResponse(Socket clientSocket, byte[] bContent, string contentType)
        {
            sendResponse(clientSocket, bContent, "200 OK", contentType);
        }

        // Для строк
        private void sendResponse(Socket clientSocket, string strContent, string responseCode, string contentType)
        {
            byte[] bContent = charEncoder.GetBytes(strContent);
            sendResponse(clientSocket, bContent, responseCode, contentType);
        }

        // Для массива байтов
        private void sendResponse(Socket clientSocket, byte[] bContent, string responseCode, string contentType)
        {
            try
            {
                byte[] bHeader = charEncoder.GetBytes(
                                    "HTTP/1.1 " + responseCode + "\r\n"
                                  + "Server: Наш простой ВЭБсервер\r\n"
                                  + "Content-Length: " + bContent.Length.ToString() + "\r\n"
                                  + "Connection: close\r\n"
                                  + "Content-Type: " + contentType + "\r\n\r\n");
                clientSocket.Send(bHeader);
                clientSocket.Send(bContent);
                clientSocket.Close();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
