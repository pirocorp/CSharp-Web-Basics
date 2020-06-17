namespace IRunes.Services
{
    using System;
    using System.Collections.Generic;
    using IRunes.Models;

    public interface IAlbumsService
    {
        void Create(string name, string cover);

        IEnumerable<T> GetAll<T>(Func<Album, T> selectFunc);

        T GetDetails<T>(string id, Func<Album, T> selectFunc);
    }
}
