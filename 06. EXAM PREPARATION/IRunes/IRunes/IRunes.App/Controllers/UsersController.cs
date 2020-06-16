namespace IRunes.App.Controllers
{
    using Services;
    using SIS.HTTP;
    using SIS.MvcFramework;
    using ViewModels.Users;

    public class UsersController : Controller
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            this._usersService = usersService;
        }

        public HttpResponse Login()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(LoginInputModel model)
        {
            return null;
        }

        public HttpResponse Register()
        {
            return this.View();
        }
    }
}
