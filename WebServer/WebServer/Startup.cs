﻿namespace WebServer
{
    using System.Threading.Tasks;
    using Server.Responses;
    using WebServer.Server;

    public static class Startup
    {
        public static async Task Main()
            => await new HttpServer( 
                        routes => routes
                            .MapGet("/", new TextResponse("Hello World!"))
                            .MapGet("/Cats", new TextResponse("Hello from the cats!"))
                            .MapGet("/Dogs", new TextResponse("Bark!!")))
                    .Start();
    }
}
