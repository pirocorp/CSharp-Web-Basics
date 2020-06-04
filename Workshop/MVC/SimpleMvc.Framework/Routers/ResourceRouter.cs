namespace SimpleMvc.Framework.Routers
{
    using System.IO;
    using System.Linq;
    using WebServer.Contracts;
    using WebServer.Enums;
    using WebServer.Exceptions;
    using WebServer.Http.Contracts;
    using WebServer.Http.Response;

    public class ResourceRouter : IHandleable
    {
        public IHttpResponse Handle(IHttpRequest request)
        {
            var fileName = request.Path.Split('/').Last();
            var fileExtension = fileName.Split('.').Last();

            try
            {
                var fileContent = this.ReadFile(fileName, fileExtension);

                return new FileResponse(HttpStatusCode.Found, fileContent);
            }
            catch
            {
                return new NotFoundResponse();
            }
        }

        private byte[] ReadFile(string fileName, string fileExtension)
        {
            var filePath = $"./{MvcContext.Get.ResourcesFolder}/{fileExtension}/{fileName}";
            if (!File.Exists(filePath))
            {
                BadRequestException.ThrowFromInvalidRequest();
            }

            return File.ReadAllBytes(filePath);
        }
    }
}
