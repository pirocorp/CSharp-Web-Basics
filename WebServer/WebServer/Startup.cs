namespace WebServer
{
    using System.Threading.Tasks;

    using WebServer.Server;

    public static class Startup
    {
        public static async Task Main()
        {
            // http://localhost:5001

            var server = new HttpServer("127.0.0.1");
            await server.Start();
        }
    }
}
