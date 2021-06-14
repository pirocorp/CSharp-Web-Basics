namespace WebServer.Server.Controllers
{
    using Http;

    public class HttpGetAttribute : HttpMethodAttribute
    {
        public HttpGetAttribute() 
            : base(HttpMethod.Get)
        {
        }
    }
}
