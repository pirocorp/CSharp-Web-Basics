namespace WebServer.Server.Http
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Collections;

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

        public QueryCollection Query { get; private set; }

        public HeaderCollection Headers { get; private set; }

        public CookieCollection Cookies { get; private set; }

        public FormCollection Form { get; private set; }

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

        private static HeaderCollection ParseHeaders(IEnumerable<string> headerLines)
        {
            var headersCollection = new HeaderCollection();

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

        private static CookieCollection ParseCookies(HeaderCollection headers)
        {
            var cookieCollection = new CookieCollection();

            if (headers.Contains(HttpHeader.Cookie))
            {
                var cookieHeader = headers[HttpHeader.Cookie];

                var allCookies = cookieHeader.Split(';');

                foreach (var cookieText in allCookies)
                {
                    var cookieParts = cookieText.Split('=');

                    var cookieName = cookieParts[0].Trim();
                    var cookieValue = cookieParts[1].Trim();

                    cookieCollection.Add(cookieName, cookieValue);
                }
            }

            return cookieCollection;
        }

        private static QueryCollection ParseQuery(string queryString)
        {
            var queryCollection = new QueryCollection();

            var parsedResult = ParseQueryString(queryString);

            foreach (var (name, value) in parsedResult)
            {
                queryCollection.Add(name, value);
            }

            return queryCollection;
        }

        private static (string Path, QueryCollection Query) ParseUrl(string url)
        {
            var urlParts = url.Split("?", 2);

            var path = urlParts[0].ToLower();
            var query = urlParts.Length > 1
                ? ParseQuery(urlParts[1])
                : new QueryCollection();

            return (path, query);
        }

        private static HttpSession GetSession(CookieCollection cookies)
        {
            var sessionId = cookies.Contains(HttpSession.SessionCookieName)
                ? cookies[HttpSession.SessionCookieName]
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

        private static FormCollection ParseForm(HeaderCollection headers, string body)
        {
            var formCollection = new FormCollection();

            if (headers.Contains(HttpHeader.ContentType)
                && headers[HttpHeader.ContentType] == HttpContentType.FormUrlEncoded)
            {
                var parsedResult = ParseQueryString(body);

                foreach (var (name, value) in parsedResult)
                {
                    formCollection.Add(name, value);
                }
            }

            return formCollection;
        }

        private static Dictionary<string, string> ParseQueryString(string queryString)
            => HttpUtility.UrlDecode(queryString)
                .Split('&')
                .Select(part => part.Split('='))
                .Where(part => part.Length == 2)
                .ToDictionary(
                    part => part[0],
                    part => part[1],
                    StringComparer.InvariantCultureIgnoreCase);
    }
}
