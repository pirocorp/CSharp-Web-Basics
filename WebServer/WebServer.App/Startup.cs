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
                        .MapStaticFiles()
                        .MapControllers()
                        .MapGet<HomeController>("/local", c => c.ToLocal()))
                .Start();
    }
}
