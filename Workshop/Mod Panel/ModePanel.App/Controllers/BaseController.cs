namespace ModePanel.App.Controllers
{
    using System.Linq;

    using Data;
    using Data.Models;
    using SimpleMvc.Framework.Controllers;

    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
            this.ViewModel["show-error"] = "none";

            this.ViewModel["anonymousDisplay"] = "inherit";
            this.ViewModel["userDisplay"] = "none";
            this.ViewModel["adminDisplay"] = "none";
        }

        protected User Profile { get; private set; }

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

                bool isAdmin;

                using (var db = new ModePanelDbContext())
                {
                    this.Profile = db.Users
                        .First(u => u.Email == this.User.Name);
                }

                if (this.Profile.IsAdmin)
                {
                    this.ViewModel["adminDisplay"] = "inherit";
                }
            }
        }

        protected bool IsAdmin => this.User.IsAuthenticated && this.Profile.IsAdmin;
    }
}
