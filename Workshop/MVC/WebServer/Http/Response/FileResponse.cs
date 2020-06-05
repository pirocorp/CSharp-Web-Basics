namespace WebServer.Http.Response
{
    using System;
    using Common;
    using Enums;

    public class FileResponse : HttpResponse
    {
        public FileResponse(HttpStatusCode statusCode, byte[] fileContent)
        {
            CoreValidator.ThrowIfNull(fileContent, nameof(fileContent));
            this.ValidateStatusCode(statusCode);

            this.StatusCode = statusCode;
            this.FileData = fileContent;

            //this.Headers.Add(HttpHeader.ContentType, "image/x-icon");
            this.Headers.Add(HttpHeader.ContentLength, fileContent.Length.ToString());
            this.Headers.Add(HttpHeader.ContentDisposition, "attachment");
        }

        public byte[] FileData { get; }

        private void ValidateStatusCode(HttpStatusCode statusCode)
        {
            var statusCodeNumber = (int) statusCode;

            if (300 > statusCodeNumber || statusCodeNumber > 400)
            {
                throw new InvalidOperationException("File responses need to have status code between 300 and 399");
            }
        }
    }
}
