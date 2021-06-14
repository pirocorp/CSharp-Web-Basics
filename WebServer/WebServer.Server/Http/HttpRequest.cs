namespace WebServer.Server.Http
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class HttpRequest
    {
        private static readonly IDictionary<string, HttpSession> SessionsStore;

        private const string NewLine = "\r\n";

        static HttpRequest()
        {
            SessionsStore = new Dictionary<string, HttpSession>();
        }

        public HttpMethod Method { get; private init; }

        public string Path { get; private init; }

        public IReadOnlyDictionary<string, string> Query { get; private init; }

        public IReadOnlyDictionary<string, HttpHeader> Headers { get; private init; }

        public IReadOnlyDictionary<string, HttpCookie> Cookies { get; private init; }

        public IReadOnlyDictionary<string, string> Form { get; private init; }

        public string Body { get; private init; }

        public HttpSession Session { get; private set; }

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

            var headers = ParseHeaders(lines.Skip(1));

            var cookies = ParseCookies(headers);

            var session = GetSession(cookies);

            var body = string.Join(NewLine, lines.Skip(2 + headers.Count));

            var (path, query) = ParseUrl(url);

            var form = ParseForm(headers, body);

            return new HttpRequest()
            {
                Method = method,
                Path = path,
                Query = query,
                Headers = headers,
                Cookies = cookies,
                Session = session,
                Body = body,
                Form = form,
            };
        }

        private static Dictionary<string, HttpHeader> ParseHeaders(IEnumerable<string> headerLines)
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

                headersCollection[name] = header;
            }

            return headersCollection;
        }

        private static IReadOnlyDictionary<string, HttpCookie> ParseCookies(Dictionary<string, HttpHeader> headers)
        {
            var cookieCollection = new Dictionary<string, HttpCookie>();

            if (!headers.ContainsKey(HttpHeader.Cookie))
            {
                return cookieCollection;
            }

            headers[HttpHeader.Cookie].Value
                .Split(";")
                .Select(c => c.Split("=", 2))
                .Where(cp => cp.Length == 2)
                .Select(cp => new
                {
                    Name = cp[0].Trim(),
                    Value = cp[1].Trim()
                })
                .ToList()
                .ForEach(c => cookieCollection[c.Name] = new HttpCookie(c.Name, c.Value));

            return cookieCollection;
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

        private static HttpSession GetSession(IReadOnlyDictionary<string, HttpCookie> cookies)
        {
            var sessionId = cookies.ContainsKey(HttpSession.SessionCookieName)
                ? cookies[HttpSession.SessionCookieName].Value
                : Guid.NewGuid().ToString();

            if (!SessionsStore.ContainsKey(sessionId))
            {
                SessionsStore[sessionId] = new HttpSession(sessionId)
                {
                    IsNew = true
                };
            }

            return SessionsStore[sessionId];
        }

        private static Dictionary<string, string> ParseForm(Dictionary<string, HttpHeader> headers, string body)
        {
            var result = new Dictionary<string, string>();

            if (headers.ContainsKey(HttpHeader.ContentType)
                && headers[HttpHeader.ContentType].Value == HttpContentType.FormUrlEncoded)
            {
                result = ParseQuery(body);
            }

            return result;
        }
    }
}
