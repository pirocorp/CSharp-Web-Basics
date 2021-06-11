namespace WebServer.Server.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Http;
    using Results;

    public class RoutingTable : IRoutingTable
    {
        private readonly Dictionary<HttpMethod, Dictionary<string, Func<HttpRequest, HttpResponse>>> routeTable;

        public RoutingTable()
        {
            this.routeTable = new ();

            Enum.GetValues<HttpMethod>()
                .ToList()
                .ForEach(httpMethod => this.routeTable[httpMethod] = new ());
        }

        public IRoutingTable Map(HttpMethod method, string path, HttpResponse response)
        {
            Guard.AgainstNull(response, nameof(response));

            return this.Map(method, path, request => response);
        }

        public IRoutingTable Map(HttpMethod method, string path, Func<HttpRequest, HttpResponse> responseFunction)
        {
            Guard.AgainstNull(path, nameof(path));
            Guard.AgainstNull(responseFunction, nameof(responseFunction));

            path = path.ToLower();

            this.routeTable[method][path.ToLower()] = responseFunction;
            return this;
        }

        public IRoutingTable MapGet(string path, HttpResponse response)
            => this.Map(HttpMethod.Get, path, request => response);

        public IRoutingTable MapGet(string path, Func<HttpRequest, HttpResponse> responseFunction)
            => this.Map(HttpMethod.Get, path, responseFunction);

        public IRoutingTable MapPost(string path, HttpResponse response)
            => this.Map(HttpMethod.Post, path, request => response);

        public IRoutingTable MapPost(string path, Func<HttpRequest, HttpResponse> responseFunction)
            => this.Map(HttpMethod.Post, path, responseFunction);

        public HttpResponse ExecuteRequest(HttpRequest request)
        {
            var requestMethod = request.Method;
            var requestPath = request.Path.ToLower();

            if (!this.routeTable.ContainsKey(requestMethod) ||
                !this.routeTable[requestMethod].ContainsKey(requestPath))
            {
                return new HttpResponse(HttpStatusCode.NotFound);
            }

            var responseFunction = this.routeTable[requestMethod][requestPath];
            return responseFunction(request);
        }
    }
}
