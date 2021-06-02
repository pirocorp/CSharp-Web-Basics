namespace WebServer.Server.Common
{
    using System;

    public static class Guard
    {
        public static void AgainstNull(object value, string name = null)
        {
            if (value is null)
            {
                name ??= "Value";

                throw new ArgumentNullException(name);
            }
        }
    }
}
