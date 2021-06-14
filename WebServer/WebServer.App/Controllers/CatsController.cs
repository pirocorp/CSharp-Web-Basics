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

        [HttpGet]
        public HttpResponse Create() => this.View();

        [HttpPost]
        public HttpResponse Save(string name, int age)
        {
            return this.Text($"{name} - {age}");
        }
    }
}
