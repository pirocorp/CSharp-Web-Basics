namespace WebServer.Exceptions
{
    using System;

    public class BadRequestException : Exception
    {
        private const string INVALID_REQUEST_MESSAGE = "Request is not valid.";

        public static object ThrowFromInvalidRequest()
            => throw new BadRequestException(INVALID_REQUEST_MESSAGE);

        public BadRequestException(string message)
            : base(message)
        {
        }
    }
}
