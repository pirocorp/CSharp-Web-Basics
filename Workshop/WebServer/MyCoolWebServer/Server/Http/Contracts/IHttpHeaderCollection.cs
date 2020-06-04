namespace MyCoolWebServer.Server.Http.Contracts
{
    using System.Collections.Generic;

    public interface IHttpHeaderCollection : IEnumerable<HttpHeader>
    {
        void Add(HttpHeader header);

        void Add(string key, string value);

        bool ContainsKey(string key);

        ICollection<HttpHeader> Get(string key);
    }
}
