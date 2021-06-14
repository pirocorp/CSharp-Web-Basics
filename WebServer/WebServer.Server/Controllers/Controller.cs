namespace WebServer.Server.Controllers
{
    using System.Runtime.CompilerServices;
    using Http;
    using Identity;
    using Results;

    public abstract class Controller
    {
        public const string UserSessionKey = "AuthenticatedUserId";

        private UserIdentity userIdentity;

        protected Controller()
        {
            this.Response = new HttpResponse(HttpStatusCode.OK);
        }

        protected HttpRequest Request { get; init; }

        protected HttpResponse Response { get; private init; }

        // Lazy loading
        protected UserIdentity User
        {
            get
            {
                if (this.userIdentity is null)
                {
                    this.userIdentity = this.Request.Session.ContainsKey(UserSessionKey)
                        ? new UserIdentity() { Id = this.Request.Session[UserSessionKey] }
                        : new UserIdentity();
                }

                return this.userIdentity;
            }
        }

        protected void SignIn(string userId)
        {
            this.Request.Session[UserSessionKey] = userId;
            this.userIdentity = new UserIdentity() { Id = userId };
        }

        protected void SignOut()
        {
            this.Request.Session.Remove(UserSessionKey);
            this.userIdentity = new UserIdentity();
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
