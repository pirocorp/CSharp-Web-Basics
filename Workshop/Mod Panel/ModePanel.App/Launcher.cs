namespace ModePanel.App
{
    using SimpleMvc.Framework;
    using SimpleMvc.Framework.Routers;
    using WebServer;

    public static class Launcher
    {
        public static void Main()
        {
            MvcEngine.Run(new WebServer(80, new ControllerRouter(), new ResourceRouter()));
        }
    }
}
