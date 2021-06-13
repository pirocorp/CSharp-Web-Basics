namespace WebServer.App.Controllers
{
    using System;
    using Server.Controllers;
    using Server.Http;
    using Server.Results;

    public class HomeController : Controller
    {
        public HomeController(HttpRequest request) 
            : base(request)
        {
        }

        public ActionResult Index()
            => this.Html("<h1>Hello World!</h1>");

        public ActionResult ToLocal() => this.Redirect("http://127.0.0.1:5000/cats");

        public ActionResult ToSoftUni() => this.Redirect("https://softuni.bg");

        public ActionResult StaticFiles() => this.View();

        public ActionResult Error() => throw new InvalidOperationException("Test error handling exceptions!");
    }
}
