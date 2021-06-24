namespace BattleCards.Controllers
{
    using System.Linq;

    using BattleCards.Data;
    using BattleCards.Data.Models;
    using BattleCards.Models.Users;
    using BattleCards.Services;

    using MyWebServer.Controllers;
    using MyWebServer.Http;

    public class UsersController : Controller
    {
        private readonly IValidator validator;
        private readonly IUserService userService;

        public UsersController(
            IValidator validator,
            IUserService userService)
        {
            this.validator = validator;
            this.userService = userService;
        }

        public HttpResponse Login() => this.View();

        [HttpPost]
        public HttpResponse Login(LoginUserFormModel model)
        {
            var userId = this.userService.ValidateUserCredentials(model.Username, model.Password);

            if (userId is null)
            {
                return this.Error("Invalid username or password.");
            }

            this.SignIn(userId);

            return this.Redirect("/Cards/All");
        }

        public HttpResponse Register() => this.View();

        [HttpPost]
        public HttpResponse Register(RegisterUserFormModel model)
        {
            var modelErrors = this.validator.ValidateUser(model);

            if (this.userService.UsernameAlreadyExists(model.Username))
            {
                modelErrors.Add($"Username {model.Username} already exists.");
            }

            if (this.userService.EmailAlreadyExists(model.Email))
            {
                modelErrors.Add($"Email {model.Email} already exists.");
            }

            if (modelErrors.Any())
            {
                return this.Error(modelErrors);
            }

            this.userService.RegisterUser(model.Username, model.Email, model.Password);

            return this.Redirect("/Users/Login");
        }

        public HttpResponse Logout()
        {
            this.SignOut();

            return this.Redirect("/");
        }
    }
}
