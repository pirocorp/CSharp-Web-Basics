namespace SIS.MvcFramework.Attributes.Http
{
    using HTTP.Enums;

    public class HttpGetAttribute : BaseHttpAttribute
    {
        public override HttpRequestMethod Method => HttpRequestMethod.Get;
    }
}
