namespace MyCoolWebServer.Server.Http
{
    using Common;
    using Contracts;

    public class HttpContext : IHttpContext
    {
        public HttpContext(IHttpRequest requestString)
        {
            CoreValidator.ThrowIfNull(requestString, nameof(requestString));

            this.Request = requestString;
        }

        public IHttpRequest Request { get; }
    }
}
