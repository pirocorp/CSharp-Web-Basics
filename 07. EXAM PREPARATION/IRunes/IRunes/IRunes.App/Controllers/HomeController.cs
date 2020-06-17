namespace IRunes.App.Controllers
{
    using Services;
    using SIS.HTTP;
    using SIS.MvcFramework;
    using ViewModels.Users;

    public class HomeController : Controller
    {
        private readonly IUsersService _usersService;

        public HomeController(IUsersService usersService)
        {
            this._usersService = usersService;
        }

        [HttpGet("/")]
        [HttpGet("/Home/Index")]
        public HttpResponse Index()
        {
            if (this.IsUserLoggedIn())
            {
                var model = new IndexViewModel()
                {
                    Username = this._usersService.GetUsername(this.User),
                };

                return this.View(model, "Home");
            }

            return this.View();
        }
    }
}
