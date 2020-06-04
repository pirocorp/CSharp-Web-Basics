namespace MyCoolWebServer.Server.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;

    using Contracts;
    using Enums;
    using Handlers;
    using Http.Contracts;

    public class AppRouteConfig : IAppRouteConfig
    {
        private readonly IDictionary<HttpRequestMethod, IDictionary<string, RequestHandler>> _routes;

        public AppRouteConfig()
        {
            this.AnonymousPaths = new List<string>();

            this._routes = new Dictionary<HttpRequestMethod, IDictionary<string, RequestHandler>>();

            var availableMethods = Enum
                .GetValues(typeof(HttpRequestMethod))
                .Cast<HttpRequestMethod>()
                .ToArray();

            foreach (var method in availableMethods)
            {
                this._routes[method] = new Dictionary<string, RequestHandler>();
            }
        }

        public IReadOnlyDictionary<HttpRequestMethod, IDictionary<string, RequestHandler>> Routes => this._routes.ToImmutableDictionary();
        
        public ICollection<string> AnonymousPaths { get; private set; }

        public void Get(string route, Func<IHttpRequest, IHttpResponse> handler)
        {
            this.AddRoute(route, HttpRequestMethod.Get, new RequestHandler(handler));
        }

        public void Post(string route, Func<IHttpRequest, IHttpResponse> handler)
        {
            this.AddRoute(route, HttpRequestMethod.Post, new RequestHandler(handler));
        }

        public void AddRoute(string route, HttpRequestMethod method, RequestHandler handler)
        {
            this._routes[method].Add(route, handler);
        }
    }
}
