namespace WebServer.App.Controllers
{
    using Server.Controllers;
    using Server.Http;
    using Server.Results;

    public class AccountController : Controller
    {
        public AccountController(HttpRequest request) 
            : base(request)
        {
        }

        public ActionResult ActionWithCookie()
        {
            this.Response.AddCookie("My-Cookie", "My-Value");

            return this.Text("Hello with cookie");
        }
    }
}
