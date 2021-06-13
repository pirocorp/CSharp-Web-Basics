namespace WebServer.App
{
    using System.Threading.Tasks;
    using Controllers;
    using Server;
    using Server.Controllers;

    public static class Startup
    {
        public static async Task Main()
            => await new HttpServer( 
                    routes => routes
                        .MapStaticFiles()
                        .MapGet<HomeController>("/", c => c.Index())
                        .MapGet<HomeController>("/softuni", c => c.ToSoftUni())
                        .MapGet<HomeController>("/local", c => c.ToLocal())
                        .MapGet<HomeController>("/error", c => c.Error())
                        .MapGet<HomeController>("/staticFiles", c => c.StaticFiles())
                        .MapGet<AnimalsController>("/Cats", c => c.Cats())
                        .MapGet<AnimalsController>("/Dogs", c => c.Dogs())
                        .MapGet<AnimalsController>("/Turtle", c => c.Turtles())
                        .MapGet<AnimalsController>("/Bunny", c => c.Bunnies())
                        .MapGet<CatsController>("/Cats/Create", c => c.Create())
                        .MapPost<CatsController>("/Cats/Save", c => c.Save())
                        .MapGet<AccountController>("/cookie", c => c.CookiesCheck())
                        .MapGet<AccountController>("/session", c => c.SessionCheck())
                        .MapGet<AccountController>("/login", c => c.Login())
                        .MapGet<AccountController>("/logout", c => c.Logout())
                        .MapGet<AccountController>("/authentication", c => c.AuthenticationCheck()))
                .Start();
    }
}
