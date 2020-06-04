namespace MyCoolWebServer.Server.Handlers
{
    using System;
    using Common;
    using Contracts;
    using Http;
    using Http.Contracts;

    public class RequestHandler : IRequestHandler
    {
        private readonly Func<IHttpRequest, IHttpResponse> _action;

        public RequestHandler(Func<IHttpRequest, IHttpResponse> action)
        {
            CoreValidator.ThrowIfNull(action, nameof(action));

            this._action = action;
        }

        public IHttpResponse Handle(IHttpContext context)
        {
            string sessionIdToSend = null;

            if (!context.Request.Cookies.ContainsKey(SessionStore.SessionCookieKey))
            {
                var sessionId = Guid.NewGuid().ToString();

                context.Request.Session = SessionStore.Get(sessionId);

                sessionIdToSend = sessionId;
            }

            var response = this._action(context.Request);

            if (sessionIdToSend != null)
            {
                var cookie = new HttpCookie(SessionStore.SessionCookieKey, sessionIdToSend)
                {
                    IsHttpOnly = true, 
                    Path = "/"
                };

                response.Cookies.Add(cookie);
            }

            if (!response.Headers.ContainsKey(HttpHeader.ContentType))
            {
                response.Headers.Add(HttpHeader.ContentType, "text/plain");
            }

            return response; 
        }
    }
}
