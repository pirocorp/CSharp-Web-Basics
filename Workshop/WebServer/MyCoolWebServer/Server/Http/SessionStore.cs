namespace MyCoolWebServer.Server.Http
{
    using System.Collections.Concurrent;

    public static class SessionStore
    {
        public const string SessionCookieKey = "SID";
        public const string CurrentUserKey = "^%Current_User_Session_Key%^";

        private static readonly ConcurrentDictionary<string, HttpSession> _sessions = 
            new ConcurrentDictionary<string, HttpSession>();

        public static HttpSession Get(string id) 
            => _sessions.GetOrAdd(id, _ => new HttpSession(id));
    }
}
