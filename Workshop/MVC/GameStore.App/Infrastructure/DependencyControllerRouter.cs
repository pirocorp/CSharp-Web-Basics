namespace GameStore.App.Infrastructure
{
    using System;
    using SimpleMvc.Framework.Controllers;
    using SimpleMvc.Framework.Routers;

    public class DependencyControllerRouter : ControllerRouter
    {
        private readonly IServiceProvider _serviceProvider;

        public DependencyControllerRouter(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        protected override Controller CreateController(Type controllerType)
        {
            return (Controller) this._serviceProvider.GetService(controllerType);
        }
    }
}
