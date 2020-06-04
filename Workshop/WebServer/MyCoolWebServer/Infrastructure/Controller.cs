namespace MyCoolWebServer.Infrastructure
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using Server.Enums;
    using Server.Http.Contracts;
    using Server.Http.Response;

    public abstract class Controller
    {
        private const string DEFAULT_PATH = "./{0}/Resources/{1}.html";
        private const string CONTENT_PLACEHOLDER = "{{{content}}}";

        public Controller()
        {
            this.ViewData = new Dictionary<string, string>()
            {
                ["anonymousDisplay"] = "none",
                ["authDisplay"] = "inherit",
                ["showError"] = "none",
            };
        }

        protected abstract string ApplicationDirectory { get; }

        protected IDictionary<string, string> ViewData { get; private set; }

        protected IHttpResponse FileViewResponse(string filename)
        {
            var result = this.ProcessFileHtml(filename);

            if (this.ViewData.Any())
            {
                foreach (var value in this.ViewData)
                {
                    result = result.Replace($"{{{{{{{value.Key}}}}}}}", value.Value);
                }
            }

            return new ViewResponse(HttpStatusCode.Ok, new FileView(result));
        }

        protected IHttpResponse RedirectResponse(string route)
            => new RedirectResponse(route);

        protected void ShowError(string errorMessage)
        {
            this.ViewData["showError"] = "block";
            this.ViewData["error"] = errorMessage;
        }

        protected bool ValidateModel(object model)
        {
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            if (Validator.TryValidateObject(model, context, results, true) == false)
            {
                foreach (var result in results)
                {
                    if (result != ValidationResult.Success)
                    {
                        this.ShowError(result.ErrorMessage);
                        return false;
                    }
                }
            }

            return true;
        }

        private string ProcessFileHtml(string filename)
        {
            var layoutHtml = File
                .ReadAllText(string.Format(DEFAULT_PATH, this.ApplicationDirectory, "Layout"));

            var fileHtml = File
                .ReadAllText(string.Format(DEFAULT_PATH, this.ApplicationDirectory, filename));

            var result = layoutHtml.Replace(CONTENT_PLACEHOLDER, fileHtml);

            return result;
        }
    }
}
