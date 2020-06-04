﻿namespace MyCoolWebServer.Server.Http.Contracts
{
    using System.Threading.Tasks;

    public interface IHttpSession
    {
        string Id { get; }

        object Get(string key);

        T Get<T>(string key);

        bool Contains(string key);

        void Add(string key, object value);

        void Clear();
    }
}