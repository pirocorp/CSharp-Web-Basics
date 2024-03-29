﻿namespace WebServer
{
    using Contracts;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    public class WebServer : IRunnable
    {
        private const string LOCAL_HOST_IP_ADDRESS = "127.0.0.1";

        private readonly int _port;
        private readonly IHandleable _requestHandler;
        private readonly IHandleable _resourceHandler;
        private readonly TcpListener _listener;

        private bool _isRunning;

        public WebServer(int port, IHandleable requestHandler, IHandleable resourceHandler)
        {
            this._port = port;
            //this._listener = new TcpListener(IPAddress.Parse(LOCAL_HOST_IP_ADDRESS), port);
            this._listener = new TcpListener(IPAddress.Any, port);
            
            this._requestHandler = requestHandler;
            this._resourceHandler = resourceHandler;
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
                var connectionHandler = new ConnectionHandler(client, this._requestHandler, this._resourceHandler);
                await connectionHandler.ProcessRequestAsync();
            }
        }
    }
}
