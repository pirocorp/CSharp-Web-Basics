namespace ModePanel.App.Controllers
{
    using SimpleMvc.Framework.Contracts;
    using SimpleMvc.Framework.Controllers;

    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
