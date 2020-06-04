namespace SimpleMvc.Framework.ViewEngine
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Contracts;

    public class View : IRenderable
    {
        private const string BASE_LAYOUT_FILE_NAME = "Layout";
        private const string CONTENT_PLACEHOLDER = "{{{content}}}";
        private const string HTML_EXTENSION = ".html";
        private const string LOCAL_ERROR_PATH = "./Errors/Error.html";

        private readonly string _templateFullQualifiedName;
        private readonly IDictionary<string, string> _viewData;

        public View(string templateFullQualifiedName, IDictionary<string, string> viewData)
        {
            this._templateFullQualifiedName = templateFullQualifiedName;
            this._viewData = viewData;
        }

        public string Render()
        {
            var fileHtml = this.ReadFile();

            if (this._viewData.Any())
            {
                foreach (var data in this._viewData)
                {
                    fileHtml = fileHtml.Replace($"{{{{{{{data.Key}}}}}}}", data.Value);
                }
            }

            return fileHtml;
        }

        private string GetErrorPath()
        {
            //var appDirectory = Directory.GetCurrentDirectory();

            //return appDirectory + LOCAL_ERROR_PATH;
            return LOCAL_ERROR_PATH;
        }

        private string GetErrorHtml()
        {
            var errorPath = this.GetErrorPath();
            var errorHtml = File.ReadAllText(errorPath);

            return errorHtml;
        }

        private string RenderLayoutHtml()
        {
            var layoutHtmlQualifiedName = $"{MvcContext.Get.ViewsFolder}\\{BASE_LAYOUT_FILE_NAME}{HTML_EXTENSION}";

            if (!File.Exists(layoutHtmlQualifiedName))
            {
                this._viewData.Add("error", $"Layout view ({layoutHtmlQualifiedName}) could not be found.");

                return GetErrorHtml();
            }

            return File.ReadAllText(layoutHtmlQualifiedName);
        }

        private string ReadFile()
        {
            var layoutHtml = this.RenderLayoutHtml();

            var viewFullQualifiedNameWithExtension = $"{this._templateFullQualifiedName}{HTML_EXTENSION}";

            if (!File.Exists(viewFullQualifiedNameWithExtension))
            {
                this._viewData.Add("error", $"Requested view ({viewFullQualifiedNameWithExtension}) could not be found.");

                return this.GetErrorHtml();
            }

            var viewHtml = File.ReadAllText(viewFullQualifiedNameWithExtension);

            return layoutHtml.Replace(CONTENT_PLACEHOLDER, viewHtml);
        }
    }
}
