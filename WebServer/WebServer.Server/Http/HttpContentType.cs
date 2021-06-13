namespace WebServer.Server.Http
{
    using System;

    public class HttpContentType
    {
        public static readonly string PlainText = $"text/plain; {UTF8Charset}";
        public static readonly string Html = $"text/html; {UTF8Charset}";
        public static readonly string Css = "text/css";
        public static readonly string Js = "application/javascript";
        public static readonly string Jpg = "image/jpeg";
        public static readonly string Png = "image/png";
        public static readonly string FormUrlEncoded = "application/x-www-form-urlencoded";

        private const string UTF8Charset = "charset=UTF-8";

        public static string GetByFileExtension(string fileExtension)
            => fileExtension.ToLower() switch
            {
                "css" => Css,
                "js" => Js,
                "jpg" or "jpeg" => Jpg,
                "png" => Png,
                _ => PlainText
            };
    }
}
