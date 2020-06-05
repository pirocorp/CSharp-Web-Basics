namespace GameStore.App
{
    using Data;
    using Microsoft.EntityFrameworkCore;
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
            => MvcEngine.Run(new WebServer(80, new ControllerRouter(), new ResourceRouter()));
    }
}
