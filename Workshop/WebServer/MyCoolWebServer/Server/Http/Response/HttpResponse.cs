namespace MyCoolWebServer.Server.Http.Response
{
    using System.Linq;
    using System.Text;
    using Common;
    using Contracts;
    using Enums;

    public abstract class HttpResponse : IHttpResponse
    {
        protected HttpResponse()
        {
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();
        }

       public IHttpHeaderCollection Headers { get; }

       public IHttpCookieCollection Cookies { get; }

       public HttpStatusCode StatusCode { get; protected set; }

        public override string ToString()
        {
            var response = new StringBuilder();

            var statusCode = (int)this.StatusCode;
            response.Append($"HTTP/1.1 {statusCode} {this.StatusCodeMessage}");
            response.Append(HttpConstants.NewLine);
            response.Append(this.Headers);
            response.Append(HttpConstants.NewLine);

            if (this.Cookies.Any())
            {
                response.Append(this.Cookies);
                response.Append(HttpConstants.NewLine);
            }

            response.Append(HttpConstants.NewLine);

            return response.ToString();
        }

        private string StatusCodeMessage => this.StatusCode.ToString();
    }
}
