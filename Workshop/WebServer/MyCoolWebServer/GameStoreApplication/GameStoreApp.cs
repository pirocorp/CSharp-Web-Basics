namespace MyCoolWebServer.GameStoreApplication
{
    using System;
    using System.Globalization;
    using Controllers;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Server.Contracts;
    using Server.Routing.Contracts;
    using ViewModels.Account;
    using ViewModels.Admin;

    public class GameStoreApp : IApplication
    {
        public void Config(IAppRouteConfig routeConfig)
        {
            routeConfig.AnonymousPaths.Add("/");
            routeConfig.AnonymousPaths.Add("/account/login");
            routeConfig.AnonymousPaths.Add("/account/register");

            routeConfig
                .Get(
                    "/account/register",
                    req => new AccountController(req).Register());

            routeConfig
                .Post(
                    "/account/register",
                    req => new AccountController(req).Register(
                        new RegisterViewModel()
                        {
                            Email = req.FormData["email"],
                            FullName = req.FormData["full-name"],
                            Password = req.FormData["password"],
                            ConfirmPassword = req.FormData["confirm-password"],
                        }));

            routeConfig
                .Get(
                    "/account/login",
                    req => new AccountController(req).Login());

            routeConfig
                .Post(
                    "/account/login",
                    req => new AccountController(req).Login(
                        new LoginViewModel()
                        {
                            Email = req.FormData["email"],
                            Password = req.FormData["password"],
                        }));

            routeConfig
                .Get(
                    "/account/logout",
                    req => new AccountController(req).Logout());

            routeConfig
                .Get(
                    "/admin/games/add",
                    req => new AdminController(req).Add());

            routeConfig
                .Post(
                    "/admin/games/add",
                    req => new AdminController(req).Add(new AdminAddGameViewModel()
                    {
                        Title = req.FormData["title"],
                        Description = req.FormData["description"],
                        Image = req.FormData["thumbnail"],
                        Price = decimal.Parse(req.FormData["price"]),
                        Size = double.Parse(req.FormData["size"]),
                        VideoId = req.FormData["video-id"],
                        ReleaseDate = DateTime.ParseExact(
                            req.FormData["release-date"], 
                            "yyyy-MM-dd", 
                            CultureInfo.InvariantCulture),
                    }));

            routeConfig
                .Get(
                    "/admin/games/list",
                    req => new AdminController(req).List());

            routeConfig
                .Get(
                    "/",
                    req => new HomeController(req).Index());
        }

        public void InitializeDatabase()
        {
            using (var db = new GameStoreDbContext())
            {
                db.Database.Migrate();
            }
        }
    }
}
