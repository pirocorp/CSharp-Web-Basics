namespace MyCoolWebServer.Application.Views.Home
{
    using Server.Contracts;

    public class AboutView : IView
    {
        public string View() => "<h1>About Page</h1>";
    }
}
