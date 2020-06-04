namespace MyCoolWebServer.GameStoreApplication.Controllers
{
    using ByTheCakeApplication.Data.Models;
    using Server.Http;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using Services;
    using Services.Contracts;
    using ViewModels.Account;

    public class AccountController : BaseController
    {
        private const string REGISTER_VIEW = @"./Account/Register";
        private const string LOGIN_VIEW = @"./Account/Login";

        private readonly IUserService _userService;

        public AccountController(IHttpRequest request) 
            : base(request)
        {
            this._userService = new UserService();
        }

        public IHttpResponse Register()
            => this.FileViewResponse(REGISTER_VIEW);

        public IHttpResponse Register(RegisterViewModel model)
        {
            //We have validation error in the model
            if (!this.ValidateModel(model))
            {
                return this.Register();
            }

            var success = this._userService.Create(model.Email, model.FullName, model.Password);

            if (!success)
            {
                this.ShowError("E-mail is taken.");

                return this.Register();
            }

            this.LoginUser(model.Email);
            return this.RedirectResponse(HomePath);
        }

        public IHttpResponse Login()
            => this.FileViewResponse(LOGIN_VIEW);

        public IHttpResponse Login(LoginViewModel model)
        {
            if (!this.ValidateModel(model))
            {
                return this.Login();
            }

            var success = this._userService.Find(model.Email, model.Password);

            if (!success)
            {
                this.ShowError("Invalid user details.");
                return this.Login();
            }

            this.LoginUser(model.Email);
            return this.RedirectResponse(HomePath);
        }

        public IHttpResponse Logout()
        {
            this.Request.Session.Clear();

            return this.RedirectResponse(HomePath);
        }
        
        private void LoginUser(string email)
        {
            this.Request.Session.Add(SessionStore.CurrentUserKey, email);
        }
    }
}
