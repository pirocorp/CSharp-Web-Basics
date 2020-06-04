﻿namespace MyCoolWebServer.Server.Routing
{
    using System.Collections.Generic;

    using Common;
    using Contracts;
    using Handlers;

    public class RoutingContext : IRoutingContext
    {
        public RoutingContext(RequestHandler handler, IEnumerable<string> parameters)
        {
            CoreValidator.ThrowIfNull(handler, nameof(handler));
            CoreValidator.ThrowIfNull(parameters, nameof(parameters));

            this.Parameters = parameters;
            this.Handler = handler;
        }

        public IEnumerable<string> Parameters { get; }

        public RequestHandler Handler { get; }
    }
}
