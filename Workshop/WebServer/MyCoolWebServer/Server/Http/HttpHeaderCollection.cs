namespace MyCoolWebServer.Server.Http
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Contracts;

    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly Dictionary<string, ICollection<HttpHeader>> _headers;

        public HttpHeaderCollection()
        {
            this._headers = new Dictionary<string, ICollection<HttpHeader>>();
        }

        public void Add(HttpHeader header)
        {
            CoreValidator.ThrowIfNull(header, nameof(header));

            var headerKey = header.Key;

            if (!this._headers.ContainsKey(headerKey))
            {
                this._headers[headerKey] = new List<HttpHeader>();
            }

            this._headers[headerKey].Add(header);
        }

        public void Add(string key, string value)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(value, nameof(value));

            var header = new HttpHeader(key, value);
            this.Add(header);
        }

        public bool ContainsKey(string key)
        {
            CoreValidator.ThrowIfNull(key, nameof(key));

            return this._headers.ContainsKey(key);
        }

        public ICollection<HttpHeader> Get(string key)
        {
            CoreValidator.ThrowIfNull(key, nameof(key));

            if (!this._headers.ContainsKey(key))
            {
                throw new InvalidOperationException($"The given key {key} is not present in headers collection.");
            }

            return this._headers[key];
        }

        public override string ToString() 
            => string.Join(HttpConstants.NewLine, this._headers.Values.SelectMany(x => x)).Trim();

        public IEnumerator<HttpHeader> GetEnumerator()
            => this._headers.Values.SelectMany(x => x).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
