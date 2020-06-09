namespace SimpleMvc.Framework.ViewEngine
{
    using Contracts;
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;

    public class View : IRenderable
    {
        public const string BaseLayoutFileName = "Layout";

        public const string ContentPlaceholder = "{{{content}}}";

        public const string FileExtension = ".html";

        public const string LocalErrorPath = "./Errors/Error.html";

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

        private string ReadFile()
        {
            var layoutHtml = this.ReadLayoutFile();

            var templateFullFilePath = $"{this._templateFullQualifiedName}{FileExtension}";

            if (!File.Exists(templateFullFilePath))
            {
                this._viewData["error"] = $"The requested view ({templateFullFilePath}) could not be found!";
                return this.GetErrorHtml();
            }

            var templateHtml = File.ReadAllText(templateFullFilePath);

            return layoutHtml.Replace(ContentPlaceholder, templateHtml);
        }

        private string ReadLayoutFile()
        {
            var layoutHtmlFile = string.Format(
                "{0}\\{1}{2}",
                MvcContext.Get.ViewsFolder,
                BaseLayoutFileName,
                FileExtension);

            if (!File.Exists(layoutHtmlFile))
            {
                this._viewData["error"] = $"Layout view ({layoutHtmlFile}) could not be found!";
                return this.GetErrorHtml();
            }

            return File.ReadAllText(layoutHtmlFile);
        }

        private string GetErrorPath()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var parentDirectory = Directory.GetParent(currentDirectory);
            var parentDirectoryPath = parentDirectory.FullName;

            //return $"{parentDirectoryPath}{LocalErrorPath}";

            return LocalErrorPath;
        }

        private string GetErrorHtml()
        {
            var errorPath = this.GetErrorPath();
            var errorHtml = File.ReadAllText(errorPath);
            return errorHtml;
        }
    }
}
