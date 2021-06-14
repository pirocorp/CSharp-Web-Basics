namespace WebServer.Server.Http
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Common;

    public class HttpResponse
    {
        public HttpResponse(HttpStatusCode statusCode)
        {
            this.StatusCode = statusCode;

            this.Headers = new Dictionary<string, HttpHeader>(StringComparer.InvariantCultureIgnoreCase);
            this.Cookies = new Dictionary<string, HttpCookie>(StringComparer.InvariantCultureIgnoreCase);

            this.AddHeader(HttpHeader.Server, "The Bad Server");
            this.AddHeader(HttpHeader.Date, DateTime.UtcNow.ToString("R"));
        }

        public HttpStatusCode StatusCode { get; protected set; }

        public IDictionary<string, HttpHeader> Headers { get; }

        public IDictionary<string, HttpCookie> Cookies { get; }

        public byte[] Content { get; protected set; }

        public bool HasContent => this.Content != null && this.Content.Any();

        public static HttpResponse ErrorResponse(string message)
            => new HttpResponse(HttpStatusCode.InternalServerError)
                .SetContent(message, HttpContentType.PlainText);

        public HttpResponse SetContent(string content, string contentType)
        {
            Guard.AgainstNull(content, nameof(content));
            Guard.AgainstNull(contentType, nameof(contentType));

            var contentLength = Encoding.UTF8.GetByteCount(content).ToString();

            this.AddHeader(HttpHeader.ContentType, contentType);
            this.AddHeader(HttpHeader.ContentLength, contentLength);

            this.Content = Encoding.UTF8.GetBytes(content);

            return this;
        }

        public HttpResponse SetContent(byte[] content, string contentType)
        {
            Guard.AgainstNull(content, nameof(content));
            Guard.AgainstNull(contentType, nameof(contentType));

            this.AddHeader(HttpHeader.ContentType, contentType);
            this.AddHeader(HttpHeader.ContentLength, content.Length.ToString());

            this.Content = content;

            return this;
        }

        public void AddHeader(string name, string value)
        {
            Guard.AgainstNull(name, nameof(name));
            Guard.AgainstNull(value, nameof(value));

            this.Headers[name] = new HttpHeader(name, value);
        }

        public void AddCookie(string name, string value)
        {
            Guard.AgainstNull(name, nameof(name));
            Guard.AgainstNull(value, nameof(value));

            this.Cookies[name] = new HttpCookie(name, value);
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine($"HTTP/1.1 {(int)this.StatusCode} {this.StatusCode}");

            foreach (var header in this.Headers.Values)
            {
                result.AppendLine(header.ToString());
            }

            foreach (var cookie in this.Cookies.Values)
            {
                result.AppendLine($"{HttpHeader.SetCookie}: {cookie}");
            }

            // will not work on linux
            if (this.HasContent)
            {
                result.AppendLine();
            }

            return result.ToString();
        }
    }
}
