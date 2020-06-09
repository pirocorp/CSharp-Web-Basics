namespace ModePanel.App.Infrastructure
{
    using System;
    using Controllers;
    using Data;
    using Microsoft.Extensions.DependencyInjection;
    using Services;
    using Services.Contracts;
    using SimpleMvc.Framework.Controllers;
    using SimpleMvc.Framework.Routers;

    public class DependencyControllerRouter : ControllerRouter
    {
        private IServiceProvider _serviceProvider;

        public DependencyControllerRouter()
        {
            this.ConfigureServicesProvider();
        }

        protected override Controller CreateController(Type controllerType)
        {
            return (Controller) this._serviceProvider.GetService(controllerType);
        }

        private void ConfigureServicesProvider()
        {
            var serviceCollection = new ServiceCollection();
            
            serviceCollection.AddDbContext<ModePanelDbContext>();

            serviceCollection.AddTransient<ILogService, LogService>();
            serviceCollection.AddTransient<IPostService, PostService>();
            serviceCollection.AddTransient<IUserService, UserService>();

            serviceCollection.AddTransient<AdminController>();
            serviceCollection.AddTransient<HomeController>();
            serviceCollection.AddTransient<PostsController>();
            serviceCollection.AddTransient<UsersController>();

            this._serviceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
