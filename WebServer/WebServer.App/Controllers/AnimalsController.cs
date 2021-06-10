namespace WebServer.App.Controllers
{
    using Server.Controllers;
    using Server.Http;

    public class AnimalsController : Controller
    {
        public AnimalsController(HttpRequest request) 
            : base(request)
        {
        }

        public HttpResponse Cats()
        {
            const string nameKey = "name";

            var query = this.Request.Query;

            var catName = query.ContainsKey(nameKey)
                ? query[nameKey]
                : "the cats";

            var result = $"<h1>Hello from {catName}!<h1>";

            return this.Html(result);
        }

        public HttpResponse Dogs() => this.View();

        public HttpResponse Bunnies() => this.View("Rabbits");

        public HttpResponse Turtles() => this.View("Animals/Wild/Turtles2");
    }
}
