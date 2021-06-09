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

        public string Path { get; private init; }

        public Dictionary<string, string> Query { get; private init; }

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

            var (path, query) = ParseUrl(url);

            return new HttpRequest()
            {
                Body = body,
                Headers = headers,
                Path = path,
                Method = method,
                Query = query
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

                var name = headerParts[0];
                var value = headerParts[1].Trim();

                headersCollection.Add(name, value);
            }

            return headersCollection;
        }

        private static Dictionary<string, string> ParseQuery(string queryString)
            => queryString
                .Split("&")
                .Select(p => p.Split("="))
                .Where(part => part.Length == 2)
                .ToDictionary(p => p[0], p => p[1]);

        private static (string Path, Dictionary<string,string> Query) ParseUrl(string url)
        {
            var urlParts = url.Split("?", 2);

            var path = urlParts[0].ToLower();
            var query = urlParts.Length > 1
                ? ParseQuery(urlParts[1])
                : new Dictionary<string, string>();

            return (path, query);
        }
    }
}
