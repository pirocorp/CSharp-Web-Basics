namespace MyCoolWebServer.ByTheCakeApplication.Controllers
{
    using Infrastructure;
    using Server.Http.Contracts;

    public class HomeController : BaseController
    {
        public IHttpResponse Index() => this.FileViewResponse("home/Index");

        public IHttpResponse About() => this.FileViewResponse("home/About");
    }
}
