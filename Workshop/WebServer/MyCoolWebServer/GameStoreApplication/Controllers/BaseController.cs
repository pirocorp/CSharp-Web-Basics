namespace MyCoolWebServer.GameStoreApplication.Controllers
{
    using Common;
    using Infrastructure;
    using Server.Http;
    using Server.Http.Contracts;
    using Services;
    using Services.Contracts;

    public abstract class BaseController : Controller
    {
        protected const string HomePath = "/";

        private readonly IUserService _userService;

        protected BaseController(IHttpRequest request)
        {
            this.Request = request;
            this.Authentication = new Authentication(false, false);
            this._userService = new UserService();

            this.ApplyAuthenticationViewData();
        }

        protected IHttpRequest Request { get; }

        protected Authentication Authentication { get; private set; }

        protected override string ApplicationDirectory => "GameStoreApplication";
        
        private void ApplyAuthenticationViewData()
        {
            var anonymousDisplay = "inherit";
            var authDisplay = "none";
            var adminDisplay = "none";

            var authenticatedUserEmail = this.Request
                .Session
                .Get<string>(SessionStore.CurrentUserKey);

            if (authenticatedUserEmail != null)
            {
                anonymousDisplay = "none";
                authDisplay = "inherit";

                var isAdmin = this._userService.IsAdmin(authenticatedUserEmail);

                if (isAdmin)
                {
                    adminDisplay = "inherit";
                }

                this.Authentication = new Authentication(true, isAdmin);
            }

            this.ViewData["anonymousDisplay"] = anonymousDisplay;
            this.ViewData["authDisplay"] = authDisplay;
            this.ViewData["adminDisplay"] = adminDisplay;
        }
    }
}
