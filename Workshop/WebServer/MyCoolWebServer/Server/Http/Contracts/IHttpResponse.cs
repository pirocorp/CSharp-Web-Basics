namespace MyCoolWebServer.Server.Http.Contracts
{
    using Enums;

    public interface IHttpResponse
    {
        IHttpHeaderCollection Headers { get; }

        IHttpCookieCollection Cookies { get; }

        HttpStatusCode StatusCode { get; }
    }
}
