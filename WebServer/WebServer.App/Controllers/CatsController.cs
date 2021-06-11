namespace WebServer.App.Controllers
{
    using Server.Controllers;
    using Server.Http;
    using Server.Results;

    public class CatsController : Controller
    {
        public CatsController(HttpRequest request) 
            : base(request)
        {
        }

        public ActionResult Create() => this.View();

        public ActionResult Save()
        {
            var name = this.Request.Form["Name"];
            var age = this.Request.Form["Age"];

            return this.Text($"{name} - {age}");
        }
    }
}
