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

        protected HttpResponse Text(string text)
            => new TextResult(this.Response, text);

        protected HttpResponse Html(string html)
            => new HtmlResult(this.Response, html);

        protected HttpResponse Redirect(string location)
            => new RedirectResult(this.Response, location);

        protected HttpResponse View([CallerMemberName] string viewName = default)
            => this.View(null, viewName);

        protected HttpResponse View(object model, [CallerMemberName] string viewName = default)
            => new ViewResult(this.Response, viewName, this.GetControllerName(), model);

        private string GetControllerName() => this.GetType().Name.Replace(nameof(Controller), string.Empty);
    }
}
