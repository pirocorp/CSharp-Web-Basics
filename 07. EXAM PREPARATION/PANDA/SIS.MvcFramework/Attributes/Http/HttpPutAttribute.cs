namespace SIS.MvcFramework.Attributes.Http
{
    using HTTP.Enums;

    public class HttpPutAttribute : BaseHttpAttribute
    {
        public override HttpRequestMethod Method => HttpRequestMethod.Put;
    }
}
