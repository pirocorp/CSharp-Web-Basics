namespace IRunes.App.Controllers
{
    using SIS.HTTP;
    using SIS.MvcFramework;

    public class AlbumsController : Controller
    {
        public HttpResponse All()
        {
            return this.View();
        }

        public HttpResponse Create()
        {
            return this.View();
        }

        public HttpResponse Details()
        {
            return this.View();
        }
    }
}
