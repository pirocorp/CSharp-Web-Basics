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

        public HttpResponse Save()
        {
            var name = this.Request.Form["Name"];
            var age = this.Request.Form["Age"];

            return this.Text($"{name} - {age}");
        }
    }
}
