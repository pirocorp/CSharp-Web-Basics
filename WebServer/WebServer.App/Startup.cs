namespace WebServer.App
{
    using System.Threading.Tasks;
    using Server;
    using Server.Responses;

    public static class Startup
    {
        public static async Task Main()
            => await new HttpServer( 
                    routes => routes
                        .MapGet("/", new HtmlResponse("<h1>Hello World!</h1>"))
                        .MapGet("/Cats", request =>
                        {
                            const string nameKey = "name";

                            var query = request.Query;

                            var catName = query.ContainsKey(nameKey)
                                ? query[nameKey]
                                : "the cats";

                            var result = $"<h1>Hello from {catName}!<h1>";

                            return new HtmlResponse(result);
                        })
                        .MapGet("/Dogs", new TextResponse("Bark!!")))
                .Start();
    }
}
