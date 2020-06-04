namespace MyCoolWebServer.Application.Controllers
{
    using Server.Enums;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using Views.Home;

    public class HomeController
    {
        /// <summary>
        /// GET /
        /// </summary>
        public IHttpResponse Index()
        {
            var response = new ViewResponse(HttpStatusCode.Ok, new IndexView());

            return response;
        }

        /// <summary>
        /// GET /about
        /// </summary>
        public IHttpResponse About()
        {
            return new ViewResponse(HttpStatusCode.Ok, new AboutView());
        }
    }
}
