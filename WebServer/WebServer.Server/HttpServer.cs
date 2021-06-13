namespace WebServer.Server
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.InteropServices.ComTypes;
    using System.Text;
    using System.Threading.Tasks;
    using Http;
    using Routing;

    public class HttpServer
    {
        private readonly IPAddress ipAddress;
        private readonly int port;
        private readonly TcpListener listener;

        private readonly IRoutingTable routingTable;

        public HttpServer(string ipAddress, int port, Action<IRoutingTable> routingTableConfiguration)
        {
            this.ipAddress = IPAddress.Parse(ipAddress);
            this.port = port;

            this.listener = new TcpListener(this.ipAddress, this.port);

            this.routingTable = new RoutingTable();
            routingTableConfiguration(this.routingTable);
        }

        public HttpServer(int port, Action<IRoutingTable> route)
            : this("127.0.0.1", port, route)
        {
        }

        public HttpServer(Action<IRoutingTable> route)
            : this(5000, route)
        {
        }

        public async Task Start()
        {
            this.listener.Start();

            Console.WriteLine($"Server started on http://{this.ipAddress}:{this.port}...");
            Console.WriteLine("Listening for requests...");

            while (true)
            {
                var connection = await this.listener.AcceptTcpClientAsync();

                _ = Task.Run(async () =>
                {
                    var networkStream = connection.GetStream();

                    var requestText = await this.ReadRequest(networkStream);

                    try
                    {
                        var request = HttpRequest.Parse(requestText);

                        var response = this.routingTable.ExecuteRequest(request);

                        this.PrepareSession(request, response);

                        this.LogPipeline(requestText, response);

                        await this.WriteResponse(networkStream, response);
                    }
                    catch (Exception exception)
                    {
                        await this.HandleError(networkStream, exception);
                    }

                    connection.Close();
                });
            }
        }

        private async Task<string> ReadRequest(NetworkStream networkStream)
        {
            var bufferLength = 1024;
            var buffer = new byte[bufferLength];

            var totalBytes = 0;
            var requestBuilder = new StringBuilder();

            do
            {
                var bytesRead = await networkStream.ReadAsync(buffer, 0, bufferLength);

                totalBytes += bytesRead;

                if (totalBytes > 10 * 1024)
                {
                    throw new InvalidOperationException("Request is too large");
                }

                requestBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            } 
            while (networkStream.DataAvailable);

            return requestBuilder.ToString();
        }

        private void PrepareSession(HttpRequest request, HttpResponse response)
            => response.AddCookie(HttpSession.SessionCookieName, request.Session.Id);

        private async Task HandleError(NetworkStream networkStream, Exception exception)
        {
            var errorMessage = $"{exception.Message}{Environment.NewLine}{exception.StackTrace}";

            var errorResponse = HttpResponse.ErrorResponse(errorMessage);

            await this.WriteResponse(networkStream, errorResponse);
        }

        private void LogPipeline(string request, HttpResponse response)
        {
            var separator = new string('-', 50);

            var log = new StringBuilder();

            log.AppendLine();
            log.AppendLine(separator);

            log.AppendLine("REQUEST");
            log.AppendLine(request);

            log.AppendLine();

            log.AppendLine("RESPONSE");
            log.AppendLine(response.ToString());

            Console.WriteLine(log);
        }

        private async Task WriteResponse(NetworkStream networkStream, HttpResponse response)
        {
            var responseBytes = Encoding.UTF8.GetBytes(response.ToString());
            await networkStream.WriteAsync(responseBytes);
        }
    }
}
