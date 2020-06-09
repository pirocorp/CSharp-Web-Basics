namespace ModePanel.App.Controllers
{
    using Data.Models;
    using Services.Contracts;
    using SimpleMvc.Framework.Contracts;
    using SimpleMvc.Framework.Controllers;

    public abstract class BaseController : Controller
    {
        protected readonly ILogService logService;
        protected readonly IUserService userService;

        protected BaseController(ILogService logService, IUserService userService)
        {
            this.ViewModel["show-error"] = "none";

            this.ViewModel["anonymousDisplay"] = "inherit";
            this.ViewModel["userDisplay"] = "none";
            this.ViewModel["adminDisplay"] = "none";

            this.logService = logService;
            this.userService = userService;
        }

        protected User Profile { get; private set; }

        protected bool IsAdmin => this.User.IsAuthenticated && this.Profile.IsAdmin;

        protected void ShowError(string error)
        {
            this.ViewModel["show-error"] = "block";
            this.ViewModel["error"] = error;
        }

        protected override void InitializeController()
        {
            base.InitializeController();

            if (this.User.IsAuthenticated)
            {
                this.ViewModel["anonymousDisplay"] = "none";
                this.ViewModel["userDisplay"] = "inherit";

                this.Profile = this.userService.GetUserProfile(this.User.Name);

                if (this.Profile.IsAdmin)
                {
                    this.ViewModel["adminDisplay"] = "inherit";
                }
            }
        }

        protected IActionResult RedirectToLogin() => this.Redirect("/users/login");

        protected IActionResult RedirectToHome() => this.Redirect("/");

        protected void Log(LogType type, string additionalInfo)
            => this.logService.Create(this.Profile.Email, type, additionalInfo);
    }
}
