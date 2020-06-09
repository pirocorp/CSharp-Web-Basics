namespace WebServer
{
    using Common;
    using Http;
    using Http.Contracts;
    using System;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using Contracts;
    using System.Linq;
    using Http.Response;

    public class ConnectionHandler
    {
        private readonly Socket _client;
        private readonly IHandleable _mvcRequestHandler;
        private readonly IHandleable _resourceHandler;

        public ConnectionHandler(
            Socket client, 
            IHandleable mvcRequestHandler,
            IHandleable resourceHandler)
        {
            CoreValidator.ThrowIfNull(client, nameof(client));
            CoreValidator.ThrowIfNull(mvcRequestHandler, nameof(mvcRequestHandler));
            CoreValidator.ThrowIfNull(resourceHandler, nameof(resourceHandler));

            this._client = client;
            this._mvcRequestHandler = mvcRequestHandler;
            this._resourceHandler = resourceHandler;
        }

        public async Task ProcessRequestAsync()
        {
            var httpRequest = this.ReadRequest();

            if (httpRequest != null)
            {
                var httpResponse = this.HandleRequest(httpRequest);

                var responseBytes = this.GetResponseBytes(httpResponse);

                var byteSegments = new ArraySegment<byte>(responseBytes);

                await this._client.SendAsync(byteSegments, SocketFlags.None);

                Console.WriteLine($"-----REQUEST-----");
                Console.WriteLine(httpRequest);
                Console.WriteLine($"-----RESPONSE-----");
                Console.WriteLine(httpResponse);
                Console.WriteLine();
            }
            
            this._client.Shutdown(SocketShutdown.Both);
        }

        private IHttpRequest ReadRequest()
        {
            var result = new StringBuilder();
            
            var data = new ArraySegment<byte>(new byte[1024]);
            
            while (true)
            {
                int numberOfBytesRead = this._client.Receive(data.Array, SocketFlags.None);

                if (numberOfBytesRead == 0)
                {
                    break;
                }

                var bytesAsString = Encoding.UTF8.GetString(data.Array, 0, numberOfBytesRead);

                result.Append(bytesAsString);

                if (numberOfBytesRead < 1023)
                {
                    break;
                }
            }

            if (result.Length == 0)
            {
                return null;
            }
            
            return new HttpRequest(result.ToString());
        }

        private IHttpResponse HandleRequest(IHttpRequest httpRequest)
        {
            if (httpRequest.Path.Contains("."))
            {
                return this._resourceHandler.Handle(httpRequest);
            }
            else
            {
                string sessionIdToSend = this.SetRequestSession(httpRequest);
                var response = this._mvcRequestHandler.Handle(httpRequest);
                this.SetResponseSession(response, sessionIdToSend);
                return response;
            }
        }

        private string SetRequestSession(IHttpRequest request)
        {
            if (!request.Cookies.ContainsKey(SessionStore.SessionCookieKey))
            {
                var sessionId = Guid.NewGuid().ToString();

                request.Session = SessionStore.Get(sessionId);

                return sessionId;
            }

            return null;
        }

        private void SetResponseSession(IHttpResponse response, string sessionIdToSend)
        {
            if (sessionIdToSend != null)
            {
                response.Headers.Add(
                    HttpHeader.SetCookie,
                    $"{SessionStore.SessionCookieKey}={sessionIdToSend}; HttpOnly; path=/");
            }
        }


        private byte[] GetResponseBytes(IHttpResponse httpResponse)
        {
            var responseBytes = Encoding.UTF8
                .GetBytes(httpResponse.ToString())
                .ToList();

            var fileResponse = httpResponse as FileResponse;
            if (fileResponse != null)
            {
                responseBytes.AddRange(fileResponse.FileData);
            }

            return responseBytes.ToArray();
        }
    }
}
