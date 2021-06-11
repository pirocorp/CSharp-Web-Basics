﻿namespace WebServer.Server.Results
{
    using Http;

    public class RedirectResult : ActionResult
    {
        public RedirectResult(HttpResponse response, string location) 
            : base(response)
        {
            this.StatusCode = HttpStatusCode.Found;
            this.Headers.Add(HttpHeader.Location, new HttpHeader(HttpHeader.Location, location));
        }
    }
}