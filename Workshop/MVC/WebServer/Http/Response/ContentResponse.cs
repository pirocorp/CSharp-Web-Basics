﻿namespace WebServer.Http.Response
{
    using Enums;
    using Exceptions;

    public class ContentResponse : HttpResponse
    {
        private readonly string _content;

        public ContentResponse(HttpStatusCode statusCode, string content)
        {
            this.ValidateStatusCode(statusCode);

            this._content = content;
            this.StatusCode = statusCode;

            this.Headers.Add(HttpHeader.ContentType, "text/html");
        }

        private void ValidateStatusCode(HttpStatusCode statusCode)
        {
            var statusCodeNumber = (int)statusCode;

            if (299 < statusCodeNumber && statusCodeNumber < 400)
            {
                throw new InvalidResponseException("View responses need a status code below 300 and above 400 (inclusive).");
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()}{this._content}";
        }
    }
}
