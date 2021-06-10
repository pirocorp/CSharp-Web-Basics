namespace WebServer.Server.Responses
{
    using Http;

    public class TextResponse : ContentResponse
    {
        public TextResponse(string content)
            : base(content, HttpContentType.PlainText)
        {
        }
    }
}
