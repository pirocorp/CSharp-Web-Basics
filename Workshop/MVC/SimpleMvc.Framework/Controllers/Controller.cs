namespace SimpleMvc.Framework.Controllers
{
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    using ActionResults;
    using Attributes.Validation;
    using Contracts;
    using Helpers;
    using Models;
    using Security;
    using ViewEngine;
    using WebServer.Http;
    using WebServer.Http.Contracts;

    public abstract class Controller
    {
        protected Controller()
        {
            this.ViewModel = new ViewModel();
            this.User = new Authentication();
        }

        protected ViewModel ViewModel { get; }

        protected internal IHttpRequest Request { get; internal set; }

        protected internal Authentication User { get; private set; }

        protected IViewable View([CallerMemberName] string caller = default)
        {
            var controllerName = ControllerHelpers.GetControllerName(this);

            var viewFullQualifiedName = $"./{MvcContext.Get.ViewsFolder}/{controllerName}/{caller}";

            var view = new View(viewFullQualifiedName, this.ViewModel.Data);

            return new ViewResult(view);
        }

        protected IRedirectable Redirect(string redirectUrl)
            => new RedirectResult(redirectUrl);

        public IActionResult NotFound()
            => new NotFoundResult();

        protected bool IsValidModel(object model)
        {
            var properties = model.GetType().GetProperties();

            foreach (var propertyInfo in properties)
            {
                var attributes = propertyInfo
                    .GetCustomAttributes()
                    .Where(a => a is PropertyValidationAttribute)
                    .Cast<PropertyValidationAttribute>()
                    .ToList();

                foreach (var attribute in attributes)
                {
                    var propertyValue = propertyInfo.GetValue(model);

                    if (!attribute.IsValid(propertyValue))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        protected void SignIn(string name)
        {
            this.Request.Session.Add(SessionStore.CurrentUserKey, name);
        }

        protected void SignOut()
        {
            this.Request.Session.Remove(SessionStore.CurrentUserKey);
        }

        protected internal virtual void InitializeController()
        {
            var user = this.Request
                .Session
                .Get<string>(SessionStore.CurrentUserKey);

            if (user != null)
            {
                this.User = new Authentication(user);
            }
        }
    }
}
