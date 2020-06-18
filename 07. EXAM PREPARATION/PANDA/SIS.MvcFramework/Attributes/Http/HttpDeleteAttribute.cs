namespace SIS.MvcFramework.Attributes.Http
{
    using HTTP.Enums;

    public class HttpDeleteAttribute : BaseHttpAttribute
    {
        public override HttpRequestMethod Method => HttpRequestMethod.Delete;
    }
}
