namespace MyCoolWebServer
{
    using Server;
    using Server.Contracts;
    using Server.Routing;

    using ByTheCakeApplication;
    using GameStoreApplication;

    public class Launcher : IRunnable
    {
        public static void Main()
        {
            new Launcher().Run();
        }

        public void Run()
        {
            var mainApplication = new GameStoreApp();
            mainApplication.InitializeDatabase();

            var routeConfig = new AppRouteConfig();
            mainApplication.Config(routeConfig);

            var webServer = new WebServer(80, routeConfig);
            webServer.Run();
        }
    }
}
