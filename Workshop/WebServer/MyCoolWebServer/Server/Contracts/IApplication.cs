namespace MyCoolWebServer.Server.Contracts
{
    using Routing.Contracts;

    public interface IApplication
    {
        void Config(IAppRouteConfig appRouteConfig);
    }
}