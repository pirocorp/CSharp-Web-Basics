namespace WebServer.App.Controllers
{
    using Server.Controllers;
    using Server.Http;

    public class HomeController : Controller
    {
        public HomeController(HttpRequest request) 
            : base(request)
        {
        }

        public HttpResponse Index()
            => this.Html("<h1>Hello World!</h1>");
    }
}
