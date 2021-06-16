namespace WebServer.App.Controllers
{
    using System.Linq;
    using Data;
    using Server.Controllers;
    using Server.Http;

    public class CatsController : Controller
    {
        private readonly IData data;

        public CatsController(IData data)
        {
            this.data = data;
        }

        public HttpResponse All()
        {
            var cats = this.data.Cats.ToList();

            return this.View(cats);
        }

        public HttpResponse AllHtml()
        {
            var cats = this.data.Cats.ToList();

            var result = "<h1>All cats in the system:</h1>";
            result += "<ul>";

            foreach (var cat in cats)
            {
                result += "<li>";
                result += cat.Name + " - " + cat.Age;
                result += "</li>";
            }

            result += "</ul>";

            return this.Html(result);
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
