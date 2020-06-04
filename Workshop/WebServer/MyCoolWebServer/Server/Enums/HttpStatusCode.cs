namespace MyCoolWebServer.Server.Enums
{
    public enum HttpStatusCode
    {
        Ok = 200,
        Created = 201,
        MovedPermanently = 301,
        Found = 302,
        MovedTemporary = 303,
        TemporaryRedirect = 307,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        InternalServerError = 500,
        NotImplemented = 501,
    }
}
