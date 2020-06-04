namespace MyCoolWebServer.Server.Http.Response
{
    using Enums;
    using Exceptions;
    using Server.Contracts;

    public class ViewResponse : HttpResponse
    {
        private readonly IView _view;

        public ViewResponse(HttpStatusCode statusCode, IView view)
        {
            this.ValidateStatusCode(statusCode);

            this._view = view;
            this.StatusCode = statusCode;

            this.Headers.Add(HttpHeader.ContentType, "text/html");
        }

        public override string ToString()
        {
            return $"{base.ToString()}{this._view.View()}";
        }

        private void ValidateStatusCode(HttpStatusCode statusCode)
        {
            var code = (int) statusCode;

            if (299 < code && code < 400)
            {
                throw new InvalidResponseException("View responses need a status code below 300 and above 400(inclusive).");
            }
        }
    }
}
