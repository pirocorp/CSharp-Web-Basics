namespace GameStore.App
{
    using Controllers;
    using Data;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Services;
    using Services.Contracts;
    using SimpleMvc.Framework;
    using SimpleMvc.Framework.Routers;
    using WebServer;

    public static class Launcher
    {
        /// <summary>
        /// Static constructors are called only once per lifetime of the application 
        /// </summary>
        static Launcher()
        {
            using var db = new GameStoreDbContext();
            db.Database.Migrate();
        }

        public static void Main()
        {
            var serviceCollection = new ServiceCollection();
            var serviceProvider = ConfigureServicesProvider(serviceCollection);

            MvcEngine.Run(new WebServer(80, new DependencyControllerRouter(serviceProvider), new ResourceRouter()));
        }

        /// <summary>
        /// Configure Dependency injection container
        /// </summary>
        /// <param name="serviceCollection"></param>
        private static ServiceProvider ConfigureServicesProvider(IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<GameStoreDbContext>();

            serviceCollection.AddTransient<IGamesService, GamesService>();
            serviceCollection.AddTransient<IUsersService, UsersService>();

            serviceCollection.AddTransient<HomeController>();
            serviceCollection.AddTransient<AdminController>();
            serviceCollection.AddTransient<UsersController>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
