﻿namespace GameStore.App.Controllers
{
    using Models.Users;
    using Services.Contracts;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Contracts;

    public class UsersController : BaseController
    {
        private const string REGISTER_ERROR = "<p>Check your form for errors</p><p>Email – must contain @ sign and a period. It must be unique</p><p>Password – must be at least 6 symbols and must contain at least 1 uppercase, 1 lowercase letter and 1 digit</p><p>Confirm Password – must match the provided password</p>";
        private const string EMAIL_EXISTS_ERROR = "<p>E-mails is already taken.</p>";
        private const string LOGIN_ERROR = "<p>Invalid credentials.</p>";

        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            this._usersService = usersService;
        }

        public IActionResult Register() => this.View();

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            if (model.Password != model.ConfirmPassword
            || !this.IsValidModel(model))
            {
                this.ShowError(REGISTER_ERROR);
                return this.View();
            }

            var result = this._usersService.Create(
                model.Email, 
                model.Password,
                model.FullName);

            if (!result)
            {
                this.ShowError(EMAIL_EXISTS_ERROR);
                return this.View();
            }

            this.SignIn(model.Email);
            return this.Redirect("/");
        }

        public IActionResult Login()
            => this.View();
        
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (!this.IsValidModel(model))
            {
                this.ShowError(LOGIN_ERROR);
                return this.View();
            }

            if (!this._usersService.Exists(model.Email, model.Password))
            {
                this.ShowError(LOGIN_ERROR);
                return this.View();
            }

            this.SignIn(model.Email);
            return this.Redirect("/");
        }

        public IActionResult Logout()
        {
            this.SignOut();
            return this.Redirect("/");
        }
    }
}
