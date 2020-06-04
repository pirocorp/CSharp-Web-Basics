namespace MyCoolWebServer.Server.Http
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common;
    using Contracts;

    public class HttpSession : IHttpSession
    {
        private readonly IDictionary<string, object> _values;

        public HttpSession(string id)
        {
            CoreValidator.ThrowIfNullOrEmpty(id, nameof(id));

            this.Id = id;
            this._values = new Dictionary<string, object>();
        }

        public string Id { get; }

        public object Get(string key)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));

            if (!this._values.ContainsKey(key))
            {
                return null;
            }

            return this._values[key];
        }

        /// <summary>
        /// Unsafe cast
        /// </summary>
        /// <typeparam name="T">Return Type</typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
            => (T)this.Get(key);

        public bool Contains(string key)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));

            return this._values.ContainsKey(key);
        }

        public void Add(string key, object value)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNull(value, nameof(value));

            this._values[key] = value;
        }

        public void Clear() => this._values.Clear();
    }
}
