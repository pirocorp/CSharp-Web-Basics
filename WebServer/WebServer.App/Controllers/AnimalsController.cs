namespace WebServer.App.Controllers
{
    using Models.Animals;
    using Server.Controllers;
    using Server.Http;

    public class AnimalsController : Controller
    {
        public HttpResponse Cats()
        {
            const string nameKey = "name";
            const string ageKey = "age";

            var query = this.Request.Query;

            var catName = query.Contains(nameKey)
                ? query[nameKey]
                : "the cats";

            var catAge = query.Contains(ageKey)
                ? int.Parse(query[ageKey])
                : 0;

            var model = new CatViewModel()
            {
                Name = catName,
                Age = catAge
            };

            return this.View(model);
        }

        public HttpResponse Dogs() => this.View(new{ Name = "Sharo", Breed = "Street Perfect", Age = "5" });

        public HttpResponse Bunnies() => this.View("Rabbits");

        public HttpResponse Turtles() => this.View("Animals/Wild/Turtles2");
    }
}
