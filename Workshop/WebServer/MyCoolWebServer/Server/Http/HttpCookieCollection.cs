namespace MyCoolWebServer.Server.Http
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Contracts;

    public class HttpCookieCollection : IHttpCookieCollection
    {
        private readonly Dictionary<string, HttpCookie> _cookies;

        public HttpCookieCollection()
        {
            this._cookies = new Dictionary<string, HttpCookie>();
        }

        public void Add(HttpCookie cookie)
        {
            CoreValidator.ThrowIfNull(cookie, nameof(cookie));

            var cookieKey = cookie.Key;
            this._cookies[cookieKey] = cookie;
        }

        public void Add(string key, string value)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(value, nameof(value));

            var header = new HttpCookie(key, value);
            this.Add(header);
        }

        public bool ContainsKey(string key)
        {
            CoreValidator.ThrowIfNull(key, nameof(key));

            return this._cookies.ContainsKey(key);
        }

        public HttpCookie Get(string key)
        {
            CoreValidator.ThrowIfNull(key, nameof(key));

            if (!this._cookies.ContainsKey(key))
            {
                throw new InvalidOperationException($"The given key {key} is not present in cookies collection.");
            }

            return this._cookies[key];
        }

        public override string ToString()
            => string.Join(HttpConstants.NewLine, this._cookies.Values).Trim();

        public IEnumerator<HttpCookie> GetEnumerator()
            => this._cookies.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
