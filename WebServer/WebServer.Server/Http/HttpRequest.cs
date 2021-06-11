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
        }

        public HttpMethod Method { get; private init; }

        public string Path { get; private init; }

        public IReadOnlyDictionary<string, string> Query { get; private init; }

        public IReadOnlyDictionary<string, string> Form { get; private init; }

        public IReadOnlyDictionary<string, HttpHeader> Headers { get; private init; }

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

            var form = ParseForm(headers, body);

            return new HttpRequest()
            {
                Body = body,
                Headers = headers,
                Path = path,
                Method = method,
                Query = query,
                Form = form,
            };
        }

        private static Dictionary<string, HttpHeader> ParseHttpHeaders(IEnumerable<string> headerLines)
        {
            var headersCollection = new Dictionary<string, HttpHeader>();

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

                var header = new HttpHeader(name, value);

                headersCollection.Add(name, header);
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

        private static Dictionary<string, string> ParseForm(Dictionary<string, HttpHeader> headers, string body)
        {
            var result = new Dictionary<string, string>();

            if (headers.ContainsKey(HttpHeader.ContentType)
                && headers[HttpHeader.ContentType].Value == HttpContentType.FormUrlEncoded)
            {

            }

            return result;
        }
    }
}
