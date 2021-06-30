namespace SharedTrip
{
    using System.Threading.Tasks;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using MyWebServer;
    using MyWebServer.Controllers;
    using MyWebServer.Results.Views;
    using Services;

    public static class Startup
    {
        public static async Task Main()
            => await HttpServer
                .WithRoutes(routes => routes
                    .MapStaticFiles()
                    .MapControllers())
                .WithServices(services => services
                    .Add<ApplicationDbContext>()
                    .Add<IViewEngine, CompilationViewEngine>()
                    .Add<IPasswordHasher, PasswordHasher>()
                    .Add<IValidator, Validator>()
                    .Add<IUserService, UserService>()
                    .Add<ITripService, TripService>()
                    .Add<IUserTripService, UserTripService>())
                .WithConfiguration<ApplicationDbContext>(context => context.Database.Migrate())
                .Start();
    }
}
