namespace WebServer.Server.Http
{
    using System;
    using System.Text;

    public abstract class HttpResponse
    {
        protected HttpResponse(HttpStatusCode statusCode)
        {
            this.StatusCode = statusCode;

            this.Headers = new HttpHeaderCollection();
            this.Headers.Add("Server", "The Bad Server");
            this.Headers.Add("Date", DateTime.UtcNow.ToString("R"));
        }

        public HttpStatusCode StatusCode { get; init; }

        public HttpHeaderCollection Headers { get; }

        public string Content { get; init; }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine($"HTTP/1.1 {(int)this.StatusCode} {this.StatusCode}");

            foreach (var header in this.Headers)
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
    }
}
