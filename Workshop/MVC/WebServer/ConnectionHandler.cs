namespace WebServer
{
    using Common;
    using Http;
    using Http.Contracts;
    using System;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using Contracts;
    using Http.Response;

    public class ConnectionHandler
    {
        private readonly Socket _client;
        private readonly IHandleable _requestHandler;
        private readonly IHandleable _resourceHandler;

        public ConnectionHandler(Socket client, IHandleable requestHandler, IHandleable resourceHandler)
        {
            CoreValidator.ThrowIfNull(client, nameof(client));
            CoreValidator.ThrowIfNull(requestHandler, nameof(requestHandler));
            CoreValidator.ThrowIfNull(resourceHandler, nameof(resourceHandler));

            this._client = client;
            this._requestHandler = requestHandler;
            this._resourceHandler = resourceHandler;
        }

        public async Task ProcessRequestAsync()
        {
            var httpRequest = await this.ReadRequest();

            if (httpRequest != null)
            {
                var httpResponse = this.HandlerRequest(httpRequest);

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

        private async Task<IHttpRequest> ReadRequest()
        {
            var result = new StringBuilder();
            
            var data = new ArraySegment<byte>(new byte[1024]);
            
            while (true)
            {
                var numberOfBytesRead = await this._client.ReceiveAsync(data.Array, SocketFlags.None);

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

        private IHttpResponse HandlerRequest(IHttpRequest httpRequest)
        {
            if (httpRequest.Path.Contains("."))
            {
                return this._resourceHandler.Handle(httpRequest);
            }

            var sessionIdToSend = this.SetRequestSession(httpRequest);
            var response =  this._requestHandler.Handle(httpRequest);
            this.SetResponseSession(response, sessionIdToSend);

            return response;
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
