namespace WebServer.App.Controllers
{
    using Models.Animals;
    using Server.Controllers;
    using Server.Http;

    public class DogsController : Controller
    {
        [HttpGet]
        public HttpResponse Create() => this.View();

        [HttpPost]
        public HttpResponse Create(DogFormModel model) => this.Text($"Dog: {model.Name} - {model.Age} - {model.Breed}");
    }
}
