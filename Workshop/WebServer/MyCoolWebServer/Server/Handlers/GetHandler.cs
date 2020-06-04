namespace MyCoolWebServer.Server.Handlers
{
    using System;
    using Http.Contracts;

    public class GetHandler : RequestHandler
    {
        public GetHandler(Func<IHttpRequest, IHttpResponse> action) 
            : base(action)
        {
        }
    }
}
