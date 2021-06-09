namespace WebServer
{
    using System.Threading.Tasks;
    using Server.Responses;
    using WebServer.Server;

    public static class Startup
    {
        public static async Task Main()
            => await new HttpServer( 
                    routes => routes
                        .MapGet("/", new HtmlResponse("<h1>Hello World!</h1>"))
                        .MapGet("/Cats", new TextResponse("Hello from the cats!"))
                        .MapGet("/Dogs", new TextResponse("Bark!!")))
                .Start();
    }
}
