namespace WebServer.Server.Results
{
    using Http;

    public class BadRequestResponse : ActionResult
    {
        public BadRequestResponse(HttpResponse response) 
            : base(response)
        {
            this.StatusCode = HttpStatusCode.BadRequest;
        }
    }
}
