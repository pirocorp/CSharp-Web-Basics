namespace MyCoolWebServer.Server.Http
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using Common;
    using Contracts;
    using Enums;
    using Exceptions;

    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            CoreValidator.ThrowIfNullOrEmpty(requestString, nameof(requestString));

            this.FormData = new Dictionary<string, string>();
            this.UrlParameters = new Dictionary<string, string>();
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();

            this.ParseRequest(requestString);
        }

        public Dictionary<string, string> FormData { get; private set; }

        public IHttpHeaderCollection Headers { get; private set; }

        public IHttpCookieCollection Cookies { get; private set; }

        public string Path { get; private set; }
        
        public HttpRequestMethod Method { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, string> UrlParameters { get; private set; }

        public IHttpSession Session { get; set; }

        public void AddUrlParameters(string key, string value)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(value, nameof(value));

            this.UrlParameters[key] = value;
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.Append($"{this.Method.ToString().ToUpper()} {this.Url} HTTP/1.1");
            result.Append(HttpConstants.NewLine);
            result.Append(this.Headers);
            result.Append(HttpConstants.NewLine);
            result.Append(HttpConstants.NewLine);
            result.Append(string.Join("&", this.FormData.ToList().Select(kvp => $"{kvp.Key}={kvp.Value}")));

            return result.ToString();
        }

        private HttpRequestMethod ParseMethod(string methodName)
        {
            var isParsed =  Enum.TryParse<HttpRequestMethod>(methodName, true, out var method);

            if (!isParsed)
            {
                throw new BadRequestException("Not Supported HTTP Method");
            }

            return method;
        }

        private string ParsePath(string url)
            => url.Split(new[] { '?', '#' }, StringSplitOptions.RemoveEmptyEntries)[0];

        private void ParseHeaders(string[] requestLines)
        {
            var emptyLineIndex = Array.IndexOf(requestLines, String.Empty);

            for (var i = 1; i < emptyLineIndex; i++)
            {
                var headerParts = requestLines[i]
                    .Split(new[] {':',}, 2, StringSplitOptions.RemoveEmptyEntries);

                if (headerParts.Length != 2)
                {
                    BadRequestException.FromInvalidRequest();
                }

                var headerKey = headerParts[0];
                var headerValue = headerParts[1].Trim();

                var header = new HttpHeader(headerKey, headerValue);
    
                this.Headers.Add(header);
            }

            if (!this.Headers.ContainsKey(HttpHeader.Host))
            {
                BadRequestException.FromInvalidRequest();
            }
        }

        private void ParseCookies()
        {
            if (this.Headers.ContainsKey(HttpHeader.Cookie))
            {
                var allCookies = this.Headers.Get(HttpHeader.Cookie)
                    .Select(c => c
                        .Value
                        .Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries))
                    .SelectMany(c => c)
                    .ToList();

                foreach (var currentCookie in allCookies)
                {
                    if (string.IsNullOrWhiteSpace(currentCookie) || !currentCookie.Contains("="))
                    {
                        continue;
                    }

                    var cookieKeyValuePair = currentCookie
                        .Split(new[] {'='}, 2, StringSplitOptions.RemoveEmptyEntries);

                    if (cookieKeyValuePair.Length == 2)
                    {
                        var key = cookieKeyValuePair[0].Trim();
                        var value = cookieKeyValuePair[1].Trim();

                        var cookie = new HttpCookie(key, value, false);
                        this.Cookies.Add(cookie);
                    }
                }
            }
        }

        private void ParseQuery(string query, IDictionary<string, string> dict)
        {
            if (!query.Contains('='))
            {
                return;
            }

            var queryPairs = query
                .Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var pair in queryPairs)
            {
                var queryKvp = pair.Split(new[] { '=' }, 2, StringSplitOptions.None);

                if (queryKvp.Length != 2)
                {
                    return;
                }

                var queryKey = WebUtility.UrlDecode(queryKvp[0]);
                var queryValue = WebUtility.UrlDecode(queryKvp[1]);

                dict[queryKey] = queryValue;
            }
        }

        private void ParseParameters()
        {
            if (!this.Url.Contains('?'))
            {
                return;
            }

            var query = this.Url
                .Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries)
                .Last();

            this.ParseQuery(query, this.UrlParameters);
        }

        private void ParseFormData(string formDataLine)
        {
            if (this.Method == HttpRequestMethod.Get)
            {
                return;
            }

            this.ParseQuery(formDataLine, this.FormData);
        }

        private void SetSession()
        {

            if (this.Cookies.ContainsKey(SessionStore.SessionCookieKey))
            {
                var cookie = this.Cookies.Get(SessionStore.SessionCookieKey);
                var sessionId = cookie.Value;

                this.Session = SessionStore.Get(sessionId);
            }
        }

        private void ParseRequest(string requestString)
        {
            var requestLines = requestString
                .Split(new[] {HttpConstants.NewLine}, StringSplitOptions.None);

            if (!requestLines.Any())
            {
                BadRequestException.FromInvalidRequest();
            }

            var requestLine = requestLines.First()
                .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);


            if (requestLine.Length != 3 
                || requestLine[2].ToLower() != "http/1.1")
            {
                BadRequestException.FromInvalidRequest();
            }

            this.Method = this.ParseMethod(requestLine.First());
            this.Url = requestLine[1];
            this.Path = this.ParsePath(this.Url);

            this.ParseHeaders(requestLines);
            this.ParseCookies();
            this.ParseParameters();
            this.ParseFormData(requestLines.Last());

            this.SetSession();
        }
    }
}
