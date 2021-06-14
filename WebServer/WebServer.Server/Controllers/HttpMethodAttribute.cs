namespace WebServer.Server.Controllers
{
    using System;
    using Http;

    public abstract class HttpMethodAttribute : Attribute
    {
        protected HttpMethodAttribute(HttpMethod httpMethod)
        {
            this.HttpMethod = httpMethod;
        }

        public HttpMethod HttpMethod { get; }
    }
}
