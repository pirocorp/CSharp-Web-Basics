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

        public ActionResult Login()
        {
            // var user = this.db.Users.Find(username, password);
            //
            // if(user != null)
            // {
            //     this.SignIn(user.Id);
            //
            //     return Text("User authenticated!");
            // }
            //
            // return Text("Invalid Credentials!");

            var someUserId = Guid.NewGuid().ToString();
            this.SignIn(someUserId);

            return this.Text("User authenticated!");
        }

        public ActionResult Logout()
        {
            this.SignOut();

            return this.Text("User signed out!");
        }

        public ActionResult AuthenticationCheck()
        {
            if (this.User.IsAuthenticated)
            {
                return this.Text($"Authenticated user: {this.User.Id}");
            }

            return this.Text($"User is not authenticated!");
        }

        public ActionResult CookiesCheck()
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

        public ActionResult SessionCheck()
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
