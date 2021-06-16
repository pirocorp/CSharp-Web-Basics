namespace WebServer.App
{
    using System.Threading.Tasks;
    using Controllers;
    using Data;
    using Server;
    using Server.Controllers;

    public static class Startup
    {
        public static async Task Main()
            => await HttpServer
                .WithRoutes(routes => routes
                    .MapStaticFiles()
                    .MapControllers()
                    .MapGet<HomeController>("/local", c => c.ToLocal()))
                .WithServices(services => services
                    .Add<IData, MyDbContext>())
                .Start();
    }
}
