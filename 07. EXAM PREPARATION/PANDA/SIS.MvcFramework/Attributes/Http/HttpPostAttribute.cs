namespace SIS.MvcFramework.Attributes.Http
{
    using HTTP.Enums;

    public class HttpPostAttribute : BaseHttpAttribute
    {
        public override HttpRequestMethod Method => HttpRequestMethod.Post;
    }
}
