namespace WebServer.Server.Results
{
    using Http;

    public class NotFoundResult : ActionResult
    {
        public NotFoundResult(HttpResponse response) 
            : base(response)
        {
            this.StatusCode = HttpStatusCode.NotFound;
        }
    }
}
