namespace WebServer.App.Controllers
{
    using Server.Controllers;
    using Server.Http;

    public class CatsController : Controller
    {
        public CatsController(HttpRequest request) 
            : base(request)
        {
        }

        public HttpResponse Create() => this.View();
    }
}
