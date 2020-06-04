namespace MyCoolWebServer.Application
{
    using Controllers;
    using Server.Contracts;
    using Server.Routing.Contracts;

    public class MainApplication : IApplication
    {
        public void Config(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig
                .Get("/", request => new HomeController().Index());

            appRouteConfig
                .Get("/about", request => new HomeController().About());
        }
    }
}
