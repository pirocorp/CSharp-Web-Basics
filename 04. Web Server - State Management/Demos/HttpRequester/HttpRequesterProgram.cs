namespace HttpRequester
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public static class HttpRequesterProgram
    {
        private const string NEW_LINE = "\r\n"; //New Line in HTTP
        
        public static async Task Main()
        {
            //IPAddress.Loopback -> localhost
            var listener = new TcpListener(IPAddress.Loopback, 80);
            listener.Start();

            Console.WriteLine($"Listening on {IPAddress.Loopback}:80");

            while (true)
            {
                var tcpClient = await listener.AcceptTcpClientAsync();

                #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(async () =>
                {
                    await ProcessClientAsync(tcpClient);

                    Console.WriteLine(new string('=', Console.WindowWidth));
                    Console.WriteLine();
                });
                #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }

        private static async Task ProcessClientAsync(TcpClient tcpClient)
        {
            using var networkStream = tcpClient.GetStream();
                //1MB TODO: Use buffer 4KB
                var requestBytes = new byte[1_000_000];
                var bytesRead = await networkStream.ReadAsync(requestBytes, 0, requestBytes.Length);
                var requestString = Encoding.UTF8.GetString(requestBytes, 0, bytesRead);

                var username = Regex.Match(requestString, "Cookie: user=[^\\n]*\\n").Value;
                username = username.Substring("Cookie: user=".Length);

                var responseText = $"<h1>{username}</h1>" + $"<h2>{DateTime.Now}</h2>";

                var response = "HTTP/1.1 200 OK" + NEW_LINE +
                               "Server: PiroServer/1.0" + NEW_LINE +
                               "Content-Type: text/html" + NEW_LINE +
                               $"Set-Cookie: user=Piroman; Path=/; Expires={DateTime.UtcNow.AddMinutes(5):R}" + NEW_LINE + 
                               "Set-Cookie: lang=bg; Path=/lang" + NEW_LINE + 
                               //"Content-Disposition: attachment; filename=niki.html" download as file + newLine +
                               //"Location: https://softuni.bg" + newLine +
                               "Content-Length: " +responseText.Length + NEW_LINE +
                               NEW_LINE + 
                               responseText;

                var responseBytes = Encoding.UTF8.GetBytes(response);
                await networkStream.WriteAsync(responseBytes, 0, responseBytes.Length);

                Console.WriteLine(requestString);
        }
    }
}
