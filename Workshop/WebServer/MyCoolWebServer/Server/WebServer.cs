namespace MyCoolWebServer.Server
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    using Contracts;
    using Routing;
    using Routing.Contracts;

    public class WebServer : IRunnable
    {
        private const string LOCAL_HOST_IP_ADDRESS = "127.0.0.1";
        private readonly int _port;
        private readonly IServerRouteConfig _serverRouteConfig;
        private readonly TcpListener _listener;
        private bool _isRunning;

        public WebServer(int port, IAppRouteConfig appRouteConfig)
        {
            this._port = port;

            this._listener = new TcpListener(IPAddress.Parse(LOCAL_HOST_IP_ADDRESS), this._port);
            this._serverRouteConfig = new ServerRouteConfig(appRouteConfig);
        }

        public void Run()
        {
            this._listener.Start();
            this._isRunning = true;

            Console.WriteLine($"Server running on {LOCAL_HOST_IP_ADDRESS}:{this._port}");

            Task.Run(this.ListenLoop).GetAwaiter().GetResult();
        }

        private async Task ListenLoop()
        {
            while (this._isRunning)
            {
                var client = await this._listener.AcceptSocketAsync();
                var connectionHandler = new ConnectionHandler(client, this._serverRouteConfig);
                await connectionHandler.ProcessRequestAsync();
            }
        }
    }
}
