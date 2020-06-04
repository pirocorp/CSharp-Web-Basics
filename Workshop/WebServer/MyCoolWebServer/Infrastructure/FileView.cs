namespace MyCoolWebServer.Infrastructure
{
    using Server.Contracts;

    public class FileView : IView
    {
        private readonly string _htmlFile;

        public FileView(string htmlFile)
        {
            this._htmlFile = htmlFile;
        }

        public string View() => this._htmlFile;
    }
}
