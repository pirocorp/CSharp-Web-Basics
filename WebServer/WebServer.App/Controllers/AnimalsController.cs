namespace WebServer.App.Controllers
{
    using Models.Animals;
    using Server.Controllers;
    using Server.Http;
    using Server.Results;

    public class AnimalsController : Controller
    {
        public AnimalsController(HttpRequest request) 
            : base(request)
        {
        }

        public HttpResponse Cats()
        {
            const string nameKey = "name";
            const string ageKey = "age";

            var query = this.Request.Query;

            var catName = query.ContainsKey(nameKey)
                ? query[nameKey]
                : "the cats";

            var catAge = query.ContainsKey(ageKey)
                ? int.Parse(query[ageKey])
                : 0;

            var model = new CatViewModel()
            {
                Name = catName,
                Age = catAge
            };

            return this.View(model);
        }

        public HttpResponse Dogs() => this.View();

        public HttpResponse Bunnies() => this.View("Rabbits");

        public HttpResponse Turtles() => this.View("Animals/Wild/Turtles2");
    }
}
