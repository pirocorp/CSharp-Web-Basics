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
                        .MapGet<HomeController>("/", c => c.Index())
                        .MapGet<HomeController>("/softuni", c => c.ToSoftUni())
                        .MapGet<HomeController>("/local", c => c.ToLocal())
                        .MapGet<AnimalsController>("/Cats", c => c.Cats())
                        .MapGet<AnimalsController>("/Dogs", c => c.Dogs())
                        .MapGet<AnimalsController>("/Turtle", c => c.Turtles())
                        .MapGet<AnimalsController>("/Bunny", c => c.Bunnies())
                        .MapGet<CatsController>("/Cats/Create", c => c.Create())
                        .MapPost<CatsController>("/Cats/Save", c => c.Save())
                        .MapGet<AccountController>("/cookie", c => c.ActionWithCookie())
                        .MapGet<AccountController>("/session", c => c.ActionWithSession()))
                .Start();
    }
}
