namespace WebServer.App
{
    using System.Threading.Tasks;
    using Controllers;
    using Server;
    using Server.Controllers;

    public static class Startup
    {
        public static async Task Main()
            => await new HttpServer( 
                    routes => routes
                        .MapGet<HomeController>("/", c => c.Index())
                        .MapGet<AnimalsController>("/Cats", c => c.Cats())
                        .MapGet<AnimalsController>("/Dogs", c => c.Dogs()))
                .Start();
    }
}
