namespace MyCoolWebServer.GameStoreApplication.Common
{
    public class Authentication
    {
        public Authentication(bool isAuthenticated, bool isAdmin)
        {
            this.IsAuthenticated = isAuthenticated;
            this.IsAdmin = isAdmin;
        }

        public bool IsAuthenticated { get; }

        public bool IsAdmin { get; }
    }
}
