namespace WebServer.Server.Http
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class HttpRequest
    {
        private const string NewLine = "\r\n";

        public HttpRequest()
        {
            this.Headers = new HttpHeaderCollection();
        }

        public HttpMethod Method { get; private init; }

        public string Url { get; private init; }

        public HttpHeaderCollection Headers { get; private init; }

        public string Body { get; private init; }

        public static HttpRequest Parse(string request)
        {
            var lines = request.Split(NewLine);

            var startLine = lines.First().Split(" ");

            var success = Enum.TryParse<HttpMethod>(startLine[0], true, out var method);
            var url = startLine[1];

            if (!success)
            {
                throw new InvalidOperationException($"Method '{method}' is not supported.");
            }

            var headers = ParseHttpHeaders(lines.Skip(1));

            var body = string.Join(NewLine, lines.Skip(2 + headers.Count));

            return new HttpRequest()
            {
                Body = body,
                Headers = headers,
                Url = url,
                Method = method
            };
        }

        private static HttpHeaderCollection ParseHttpHeaders(IEnumerable<string> headerLines)
        {
            var headersCollection = new HttpHeaderCollection();

            foreach (var headerLine in headerLines)
            {
                if (headerLine == string.Empty)
                {
                    break;
                }

                var headerParts = headerLine.Split(":", 2);

                if (headerParts.Length != 2)
                {
                    throw new InvalidOperationException("Request is not valid.");
                }

                var header = new HttpHeader()
                {
                    Name = headerParts[0],
                    Value = headerParts[1].Trim(),
                };

                headersCollection.Add(header);
            }

            return headersCollection;
        }
    }
}
