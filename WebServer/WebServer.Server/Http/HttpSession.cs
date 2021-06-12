﻿namespace WebServer.Server.Http
{
    using System.Collections.Generic;
    using Common;

    public class HttpSession
    {
        public const string SessionCookieName = "Bad-Server-Session-Id";

        private readonly IDictionary<string, string> data;

        public HttpSession(string id)
        {
            Guard.AgainstNull(id, nameof(id));

            this.Id = id;
            this.data = new Dictionary<string, string>();
        }

        public int Count => this.data.Count;

        public string Id { get; private init; }

        public string this[string key]
        {
            get => this.data[key];
            set => this.data[key] = value;
        }

        public bool ContainsKey(string key) 
            => this.data.ContainsKey(key);
    }
}