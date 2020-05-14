namespace HttpRequester
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    public static class HttpRequesterProgram
    {
        public static void Main()
        {
            const string newLine = "\r\n"; //New Line in HTTP 

            //IPAddress.Loopback -> localhost
            var listener = new TcpListener(IPAddress.Loopback, 80);
            listener.Start();

            while (true)
            {
                var tcpClient = listener.AcceptTcpClient();

                using (var networkStream = tcpClient.GetStream())
                {
                    //1MB TODO: Use buffer 4KB
                    var requestBytes = new byte[1_000_000];
                    var bytesRead = networkStream.Read(requestBytes, 0, requestBytes.Length);
                    var requestString = Encoding.UTF8.GetString(requestBytes, 0, bytesRead);

                    var responseText = @"<h1>Hello World!</h1> 
<form method='POST' action='/Account/Login'> 
<input type=date name='date'/> 
<input type=text name='username'/> 
<input type=password name='password'/> 
<input type=submit value='Login'/> 
</form>";

                    var response = "HTTP/1.1 200 OK" + newLine +
                                   "Server: PiroServer/1.0" + newLine +
                                   "Content-Type: text/html" + newLine +
                                   //"Content-Disposition: attachment; filename=niki.html" download as file + newLine +
                                   //"Location: https://softuni.bg" + newLine +
                                   "Content-Length: " +responseText.Length + newLine +
                                   newLine + 
                                   responseText;

                    var responseBytes = Encoding.UTF8.GetBytes(response);
                    networkStream.Write(responseBytes, 0, responseBytes.Length);

                    Console.WriteLine(requestString);
                }

                Console.WriteLine(new string('=', Console.WindowWidth));
                Console.WriteLine();
            }
        }

        public static async Task HttpRequest()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36");
            var response = await client.GetAsync("https://softuni.bg");
            var result = await response.Content.ReadAsStringAsync();
            File.WriteAllText("index.html", result);
        }
    }
}
