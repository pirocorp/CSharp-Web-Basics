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
        public HttpResponse Login(LoginInputModel input)
        {
            var userId = this._usersService
                .GetUserId(input.Username, input.Password);

            if (userId != null)
            {
                this.SignIn(userId);

                return this.Redirect("/");
            }

            return this.Redirect("/Users/Login");
        }

        public HttpResponse Register()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(RegisterInputModel input)
        {
            if (string.IsNullOrWhiteSpace(input.Email))
            {
                return this.Error("Email can't be empty.");
            }

            if (input.Password.Length < 6
                || input.Password.Length > 20)
            {
                return this.Error("Password must be between 6 and 20 characters long.");
            }

            if (input.Username.Length < 4
                || input.Username.Length > 10)
            {
                return this.Error("Username must be between 4 and 10 characters long.");
            }

            if (input.Password != input.ConfirmPassword)
            {
                return this.Error("Passwords should match");
            }

            if (this._usersService.UsernameExists(input.Username))
            {
                return this.Error($"Username: {input.Username} is already taken.");
            }

            if (this._usersService.EmailExists(input.Email))
            {
                return this.Error($"Email: {input.Email} is already taken.");
            }

            this._usersService.Register(input.Username, input.Email, input.Password);

            var userId = this._usersService
                .GetUserId(input.Username, input.Password);
            this.SignIn(userId);

            return this.Redirect("/");
        }

        public HttpResponse Logout()
        {
            this.SignOut();

            return this.Redirect("/");
        }
    }
}
