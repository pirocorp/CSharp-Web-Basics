namespace WebServer.App.Controllers
{
    using System;
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
            const string cookieName = "My-Cookie";

            if (this.Request.Cookies.ContainsKey(cookieName))
            {
                var cookie = this.Request.Cookies[cookieName];

                return this.Text($"Hello with cookie! - {cookie}");
            }

            this.Response.AddCookie(cookieName, "My-Value");
            this.Response.AddCookie("My-Second-Cookie", "My-Second-Value");

            return this.Text("Cookies set!");
        }

        public ActionResult ActionWithSession()
        {
            const string currentDateKey = "CurrentDate";

            if (this.Request.Session.ContainsKey(currentDateKey))
            {
                var currentDate = this.Request.Session[currentDateKey];

                return this.Text($"Stored date: {currentDate}");
            }

            this.Request.Session[currentDateKey] = DateTime.UtcNow.ToString();
            return this.Text($"Current date stored!");
        }
    }
}
