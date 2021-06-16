namespace WebServer.Server.Services
{
    using System;

    public interface IServiceCollection
    {
        IServiceCollection Add<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        TService Get<TService>()
            where TService : class;

        object CreateInstance(Type type);
    }
}
