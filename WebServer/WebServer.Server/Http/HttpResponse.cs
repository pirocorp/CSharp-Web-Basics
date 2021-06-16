namespace WebServer.Server.Http
{
    using System;
    using System.Linq;
    using System.Text;
    using Collections;
    using Common;

    public class HttpResponse
    {
        public HttpResponse(HttpStatusCode statusCode)
        {
            this.StatusCode = statusCode;

            this.Headers = new HeaderCollection();
            this.Cookies = new CookieCollection();

            this.Headers.Add(HttpHeader.Server, "The Bad Server");
            this.Headers.Add(HttpHeader.Date, DateTime.UtcNow.ToString("R"));
        }

        public HttpStatusCode StatusCode { get; protected set; }

        public HeaderCollection Headers { get; }

        public CookieCollection Cookies { get; }

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

            this.Headers.Add(HttpHeader.ContentType, contentType);
            this.Headers.Add(HttpHeader.ContentLength, contentLength);

            this.Content = Encoding.UTF8.GetBytes(content);

            return this;
        }

        public HttpResponse SetContent(byte[] content, string contentType)
        {
            Guard.AgainstNull(content, nameof(content));
            Guard.AgainstNull(contentType, nameof(contentType));

            this.Headers.Add(HttpHeader.ContentType, contentType);
            this.Headers.Add(HttpHeader.ContentLength, content.Length.ToString());

            this.Content = content;

            return this;
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine($"HTTP/1.1 {(int)this.StatusCode} {this.StatusCode}");

            foreach (var header in this.Headers)
            {
                result.AppendLine(header.ToString());
            }

            foreach (var cookie in this.Cookies)
            {
                result.AppendLine($"{HttpHeader.SetCookie}: {cookie}");
            }

            if (this.HasContent)
            {
                result.AppendLine();
            }

            return result.ToString();
        }
    }
}
