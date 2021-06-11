namespace WebServer.Server.Controllers
{
    using System.Runtime.CompilerServices;
    using Http;
    using Results;

    public abstract class Controller
    {
        protected Controller(HttpRequest request)
        {
            this.Request = request;
            this.Response = new HttpResponse(HttpStatusCode.OK);
        }

        protected HttpRequest Request { get; private init; }

        public HttpResponse Response { get; private init; }    

        protected ActionResult Text(string text)
            => new TextResult(this.Response, text);

        protected ActionResult Html(string html)
            => new HtmlResult(this.Response, html);

        protected ActionResult Redirect(string location)
            => new RedirectResult(this.Response, location);

        protected ActionResult View([CallerMemberName] string viewName = default)
            => this.View(null, viewName);

        protected ActionResult View(object model, [CallerMemberName] string viewName = default)
            => new ViewResult(this.Response, viewName, this.GetControllerName(), model);

        private string GetControllerName() => this.GetType().Name.Replace(nameof(Controller), string.Empty);
    }
}
