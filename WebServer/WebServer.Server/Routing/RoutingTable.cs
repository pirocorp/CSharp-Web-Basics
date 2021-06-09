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

        public IRoutingTable Map(string url, HttpMethod method, HttpResponse response)
        {
            url = url.ToLower();

            return method switch
            {
                HttpMethod.Get => this.MapGet(url, response),
                _ => throw new InvalidOperationException($"Method {method} is not supported"),
            };
        }

        public IRoutingTable MapGet(string url, HttpResponse response)
        {
            url = url.ToLower();

            Guard.AgainstNull(url, nameof(url));
            Guard.AgainstNull(response, nameof(response));

            this.routeTable[HttpMethod.Get][url] = response;
            return this;
        }

        public HttpResponse MatchRequest(HttpRequest request)
        {
            var requestMethod = request.Method;
            var requestPath = request.Url.ToLower();

            if (!this.routeTable.ContainsKey(requestMethod) ||
                !this.routeTable[requestMethod].ContainsKey(requestPath))
            {
                return new NotFoundResponse();
            }
            
            return this.routeTable[requestMethod][requestPath];
        }
    }
}
