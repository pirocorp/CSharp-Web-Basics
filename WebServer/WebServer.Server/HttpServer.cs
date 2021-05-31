namespace WebServer.Server
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using Http;

    public class HttpServer
    {
        private readonly IPAddress ipAddress;
        private readonly int port;
        private readonly TcpListener listener;

        public HttpServer(string ipAddress, int port = 5001)
        {
            this.ipAddress = IPAddress.Parse(ipAddress);
            this.port = port;

            this.listener = new TcpListener(this.ipAddress, this.port);
        }

        public async Task Start()
        {
            this.listener.Start();

            Console.WriteLine($"Server started on http://{this.ipAddress}:{this.port}...");
            Console.WriteLine("Listening for requests...");

            while (true)
            {
                var connection = await this.listener.AcceptTcpClientAsync();
                var networkStream = connection.GetStream();

                var requestText = await this.ReadRequest(networkStream);
                Console.WriteLine(requestText);

                if (string.IsNullOrWhiteSpace(requestText))
                {
                    connection.Close();
                    continue;
                }

                var request = HttpRequest.Parse(requestText);

                await this.WriteResponse(networkStream);

                await Task.Delay(100);
                connection.Close();
            }
        }

        private async Task<string> ReadRequest(NetworkStream networkStream)
        {
            var bufferLength = 1024;
            var buffer = new byte[bufferLength];

            var totalBytes = 0;
            var requestBuilder = new StringBuilder();

            await Task.Delay(100);

            while (networkStream.DataAvailable)
            {
                var bytesRead = await networkStream.ReadAsync(buffer, 0, bufferLength);

                totalBytes += bytesRead;

                if (totalBytes > 10 * 1024)
                {
                    throw new InvalidOperationException("Request is too large");
                }

                requestBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            }

            return requestBuilder.ToString();
        }

        private async Task WriteResponse(NetworkStream networkStream)
        {
            var content = @"
<html>
    <head>
        <link rel=""icon"" href=""data:,"">
    </head>
    <body>
        <h1>Серверко Работи!</h1>
    </body>    
</html>";
            var contentLength = Encoding.UTF8.GetByteCount(content);

            var response = $@"HTTP/1.1 200 OK
Server: My Web Server
Date: {DateTime.UtcNow:R}
Content-Length: {contentLength}
Content-Type: text/html; charset=UTF-8

{content}";

            var responseBytes = Encoding.UTF8.GetBytes(response);
            await networkStream.WriteAsync(responseBytes);
        }
    }
}
