namespace SharedTrip.Controllers
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
            var user = this._usersService
                .GetUserOrDefault(input.Username, input.Password);

            if (user == null)
            {
                return this.Redirect("/Users/Login");
            }

            this.SignIn(user.Id);
            return this.Redirect("/");
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

            if (input.Username.Length < 6
                || input.Username.Length > 20)
            {
                return this.Error("Username must be between 6 and 20 characters long.");
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

            this._usersService.Create(input.Username, input.Email, input.Password);
            return this.Redirect("/Users/Login");
        }

        public HttpResponse Logout()
        {
            this.SignOut();

            return this.Redirect("/");
        }
    }
}
