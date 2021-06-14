namespace WebServer.Server.Controllers
{
    using System.Runtime.CompilerServices;
    using Http;
    using Identity;
    using Results;

    public abstract class Controller
    {
        private const string UserSessionKey = "AuthenticatedUserId";

        protected Controller(HttpRequest request)
        {
            this.Request = request;

            this.Response = new HttpResponse(HttpStatusCode.OK);
            this.User = this.Request.Session.ContainsKey(UserSessionKey)
                ? new UserIdentity() { Id = this.Request.Session[UserSessionKey] }
                : new UserIdentity();
        }

        protected HttpRequest Request { get; private init; }

        protected HttpResponse Response { get; private init; }

        protected UserIdentity User { get; private set; }

        protected void SignIn(string userId)
        {
            this.Request.Session[UserSessionKey] = userId;
            this.User = new UserIdentity() { Id = userId };
        }

        protected void SignOut()
        {
            this.Request.Session.Remove(UserSessionKey);
            this.User = new UserIdentity();
        }

        protected ActionResult Text(string text)
            => new TextResult(this.Response, text);

        protected ActionResult Html(string html)
            => new HtmlResult(this.Response, html);

        protected ActionResult Redirect(string location)
            => new RedirectResult(this.Response, location);

        protected ActionResult View([CallerMemberName] string viewName = default)
            => this.View(null, viewName);

        protected ActionResult View(object model, [CallerMemberName] string viewName = default)
            => new ViewResult(this.Response, viewName, this.GetType().GetControllerName(), model);
    }
}
