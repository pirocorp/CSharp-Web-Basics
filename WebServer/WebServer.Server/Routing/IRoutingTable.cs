namespace WebServer.Server.Routing
{
    using Http;

    public interface IRoutingTable
    {
        IRoutingTable Map(HttpMethod method, string path, HttpResponse response);

        IRoutingTable MapGet(string path, HttpResponse response);

        IRoutingTable MapPost(string path, HttpResponse response);

        HttpResponse MatchRequest(HttpRequest request);
    }
}
