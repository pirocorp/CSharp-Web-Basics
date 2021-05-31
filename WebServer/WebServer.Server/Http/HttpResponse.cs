namespace WebServer.Server.Http
{
    public class HttpResponse
    {
        public HttpStatusCode StatusCode { get; init; }

        public HttpHeaderCollection Headers { get; init; }

        public string Content { get; init; }
    }
}
