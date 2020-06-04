namespace MyCoolWebServer.Server.Exceptions
{
    using System;

    public class BadRequestException : Exception
    {
        private const string INVALID_REQUEST_MESSAGE = "Request is not valid.";

        public BadRequestException(string message)
            : base(message)
        {
        }

        public static void FromInvalidRequest() 
            => throw new BadRequestException(INVALID_REQUEST_MESSAGE);
    }
}
