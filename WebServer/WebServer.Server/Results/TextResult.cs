namespace WebServer.Server.Results
{
    using Http;

    public class TextResult : ContentResult
    {
        public TextResult(HttpResponse response, string content)
            : base(response, content, HttpContentType.PlainText)
        {
        }
    }
}
