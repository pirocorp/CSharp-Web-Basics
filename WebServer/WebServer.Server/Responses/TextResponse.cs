﻿namespace WebServer.Server.Responses
{
    using System.Text;
    using Common;
    using Http;

    public class TextResponse : HttpResponse
    {
        public TextResponse(string text, string contentType)
            : base(HttpStatusCode.OK)
        {
            Guard.AgainstNull(text, nameof(text));

            var contentLength = Encoding.UTF8.GetByteCount(text).ToString();

            this.Headers.Add("Content-Type", contentType);
            this.Headers.Add("Content-Length", contentLength);

            this.Content = text;
        }

        public TextResponse(string text)
            : this(text, "text/plain; charset=UTF-8")
        {
        }
    }
}
