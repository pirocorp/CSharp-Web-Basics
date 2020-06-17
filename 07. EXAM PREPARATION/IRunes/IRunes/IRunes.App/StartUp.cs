namespace IRunes.App
{
    using System.Collections.Generic;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Services;
    using SIS.HTTP;
    using SIS.MvcFramework;

    public class StartUp : IMvcApplication
    {
        public void Configure(IList<Route> routeTable)
        {
            var db = new ApplicationDbContext();
            db.Database.Migrate();
        }

        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.Add<IAlbumsService, AlbumsService>();
            serviceCollection.Add<ITracksService, TracksService>();
            serviceCollection.Add<IUsersService, UsersService>();
        }
    }
}
