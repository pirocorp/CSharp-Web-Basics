namespace MyCoolWebServer.Server.Http
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;
    using Common;

    public class HttpCookie
    {
        private HttpCookie()
        {
            this.IsNew = true;
        }

        /// <summary> 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expires">Is In days</param>
        public HttpCookie(string key, string value, int expires = 3)
            :this()
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(value, nameof(value));

            this.Key = key;
            this.Value = value;

            this.Expires = DateTime.UtcNow.AddDays(expires);
        }

        public HttpCookie(string key, string value, bool isNew, int expires = 3)
            : this(key, value, expires)
        {
            this.IsNew = isNew;
        }

        public string Key { get; }

        public string Value { get; }

        public DateTime Expires { get; }

        public bool IsNew { get; }

        public bool IsHttpOnly { get; set; }

        public string Path { get; set; }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.Append($"Set-Cookie: {this.Key}={this.Value}; Expires={this.Expires:R}");

            if (IsHttpOnly)
            {
                result.Append($"; HttpOnly");
            }

            if (!string.IsNullOrWhiteSpace(this.Path))
            {
                result.Append($"; path={this.Path}");
            }

            return result.ToString();
        }
    }
}
