namespace ModePanel.App.Controllers
{
    using SimpleMvc.Framework.Contracts;

    public class PostsController : BaseController
    {
        public IActionResult Create()
        {
            if (!this.User.IsAuthenticated)
            {
                return this.RedirectToLogin();
            }

            return this.View();
        }
    }
}
