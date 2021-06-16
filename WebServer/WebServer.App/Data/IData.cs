namespace WebServer.App.Data
{
    using System.Collections.Generic;
    using Models;

    public interface IData
    {
        IEnumerable<Cat> Cats { get; }
    }
}
