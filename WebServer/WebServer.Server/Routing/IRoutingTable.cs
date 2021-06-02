namespace WebServer.Server.Routing
{
    using Http;

    public interface IRoutingTable
    {
        IRoutingTable Map(string url, HttpMethod method, HttpResponse response);

        IRoutingTable MapGet(string url, HttpResponse response);

        // IRoutingTable MapPost(string url, HttpResponse response);
    }
}
