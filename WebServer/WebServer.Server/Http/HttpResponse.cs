﻿namespace WebServer.Server.Http
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Common;

    public class HttpResponse
    {
        public HttpResponse(HttpStatusCode statusCode)
        {
            this.StatusCode = statusCode;

            this.Headers = new Dictionary<string, HttpHeader>();
            this.Cookies = new Dictionary<string, HttpCookie>();

            this.Headers.Add(HttpHeader.Server, new HttpHeader(HttpHeader.Server, "The Bad Server"));
            this.Headers.Add(HttpHeader.Date, new HttpHeader(HttpHeader.Date, DateTime.UtcNow.ToString("R")));
        }

        public HttpStatusCode StatusCode { get; protected set; }

        public IDictionary<string, HttpHeader> Headers { get; }

        public IDictionary<string, HttpCookie> Cookies { get; }

        public string Content { get; protected set; }

        public void AddHeader(string name, string value)
        {
            Guard.AgainstNull(name, nameof(name));
            Guard.AgainstNull(value, nameof(value));

            this.Headers.Add(name, new HttpHeader(name, value));
        }

        public void AddCookie(string name, string value)
        {
            Guard.AgainstNull(name, nameof(name));
            Guard.AgainstNull(value, nameof(value));

            this.Cookies.Add(name, new HttpCookie(name, value));
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine($"HTTP/1.1 {(int)this.StatusCode} {this.StatusCode}");

            foreach (var header in this.Headers.Values)
            {
                result.AppendLine(header.ToString());
            }

            // will not work on linux
            if (!string.IsNullOrEmpty(this.Content))
            {
                result.AppendLine();
                result.Append(this.Content);
            }

            return result.ToString();
        }

        protected void PrepareContent(string content, string contentType)
        {
            Guard.AgainstNull(content, nameof(content));
            Guard.AgainstNull(contentType, nameof(contentType));

            var contentLength = Encoding.UTF8.GetByteCount(content).ToString();

            this.Headers.Add(HttpHeader.ContentType, new HttpHeader(HttpHeader.ContentType, contentType));
            this.Headers.Add(HttpHeader.ContentLength, new HttpHeader(HttpHeader.ContentLength, contentLength));

            this.Content = content;
        }
    }
}
