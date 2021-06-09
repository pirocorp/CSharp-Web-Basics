namespace WebServer.Server.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Http;
    using Responses;

    public class RoutingTable : IRoutingTable
    {
        private readonly Dictionary<HttpMethod, Dictionary<string, HttpResponse>> routeTable;

        public RoutingTable()
        {
            this.routeTable = new Dictionary<HttpMethod, Dictionary<string, HttpResponse>>();

            Enum.GetValues<HttpMethod>()
                .ToList()
                .ForEach(httpMethod => this.routeTable[httpMethod] = new Dictionary<string, HttpResponse>());
        }

        public IRoutingTable Map(HttpMethod method, string path, HttpResponse response)
        {
            Guard.AgainstNull(path, nameof(path));
            Guard.AgainstNull(response, nameof(response));

            path = path.ToLower();

            this.routeTable[method][path] = response;
            return this;
        }

        public IRoutingTable MapGet(string path, HttpResponse response)
            => this.Map(HttpMethod.Get, path, response);

        public IRoutingTable MapPost(string path, HttpResponse response)
            => this.Map(HttpMethod.Post, path, response);

        public HttpResponse MatchRequest(HttpRequest request)
        {
            var requestMethod = request.Method;
            var requestPath = request.Path.ToLower();

            if (!this.routeTable.ContainsKey(requestMethod) ||
                !this.routeTable[requestMethod].ContainsKey(requestPath))
            {
                return new NotFoundResponse();
            }
            
            return this.routeTable[requestMethod][requestPath];
        }
    }
}
