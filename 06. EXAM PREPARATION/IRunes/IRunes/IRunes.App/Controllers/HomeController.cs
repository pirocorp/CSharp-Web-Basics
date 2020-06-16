namespace IRunes.App.Controllers
{
    using SIS.HTTP;
    using SIS.MvcFramework;

    public class HomeController : Controller
    {
        [HttpGet("/")]
        [HttpGet("/Home/Index")]
        public HttpResponse Index()
        {
            return this.View();
        }
    }
}
