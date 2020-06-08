namespace ModePanel.App
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using SimpleMvc.Framework;
    using SimpleMvc.Framework.Routers;
    using WebServer;

    public static class Launcher
    {
        static Launcher()
        {
            using var db = new ModePanelDbContext();

            db.Database.Migrate();
        }

        public static void Main()
        {
            MvcEngine.Run(new WebServer(80, new ControllerRouter(), new ResourceRouter()));
        }
    }
}
