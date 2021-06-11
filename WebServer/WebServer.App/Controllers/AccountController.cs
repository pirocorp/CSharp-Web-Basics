namespace WebServer.App.Controllers
{
    using Server.Controllers;
    using Server.Http;

    public class AccountController : Controller
    {
        public AccountController(HttpRequest request) 
            : base(request)
        {
        }
    }
}
