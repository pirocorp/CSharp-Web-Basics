namespace MyCoolWebServer.ByTheCakeApplication.Controllers
{
    using System;
    using Infrastructure;
    using Server.Http;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using Services;
    using ViewModels;
    using ViewModels.Account;

    public class AccountController : BaseController
    {
        private const string REGISTER_VIEW = "Account/Register";
        private const string LOGIN_VIEW = "Account/Login";

        private readonly IUserService _userService;

        public AccountController()
        {
            this._userService = new UserService();
        }

        public IHttpResponse Register()
        {
            this.SetDefaultViewData();

            return this.FileViewResponse(REGISTER_VIEW);
        }

        public IHttpResponse Register(IHttpRequest request, RegisterUserViewModel model)
        {
            //Validate the model
            if (model.Username.Length < 3
                || model.Password.Length < 3
                || model.ConfirmPassword != model.Password)
            {
                this.SetDefaultViewData();
                this.ShowError("Invalid user details.");

                return this.FileViewResponse(REGISTER_VIEW);
            }

            //Call the business logic
            var success = this._userService.Create(model.Username, model.Password);

            if (!success)
            {
                this.SetDefaultViewData();
                this.ShowError("Username is already taken");

                return this.FileViewResponse(REGISTER_VIEW);
            }

            var username = model.Username;
            this.LoginUser(request, username);

            return new RedirectResponse("/");
        }

        public IHttpResponse Login()
        {
            this.SetDefaultViewData();

            return this.FileViewResponse(LOGIN_VIEW);
        }

        public IHttpResponse Login(IHttpRequest request, LoginViewModel model)
        {

            if (string.IsNullOrWhiteSpace(model.Username)
                || string.IsNullOrWhiteSpace(model.Password))
            {

                this.ShowError("Username or Password missing");

                return this.FileViewResponse(LOGIN_VIEW);
            }

            var success = this._userService.Find(model.Username, model.Password);

            if (!success)
            {
                this.ShowError("Invalid user details.");

                return this.FileViewResponse(LOGIN_VIEW);
            }

            this.LoginUser(request, model.Username);

            return new RedirectResponse("/");
        }

        public IHttpResponse Profile(IHttpRequest request)
        {
            if (!request.Session.Contains(SessionStore.CurrentUserKey))
            {
                throw new InvalidOperationException("There is no logged in user.");
            }

            var username = request.Session.Get<string>(SessionStore.CurrentUserKey);

            var profile = this._userService.Profile(username);

            if (profile == null)
            {
                throw new InvalidOperationException($"The user {username} could not be found.");
            }

            this.ViewData["username"] = profile.Username;
            this.ViewData["registrationDate"] = profile.RegistrationDate.ToShortDateString();
            this.ViewData["totalOrders"] = profile.TotalOrders.ToString();

            return this.FileViewResponse("Account/Profile");
        }

        public IHttpResponse Logout(IHttpRequest req)
        {
            req.Session.Clear();

            return new RedirectResponse("/login");
        }

        private void SetDefaultViewData () => this.ViewData["authDisplay"] = "none";

        private void LoginUser(IHttpRequest request, string username)
        {
            request.Session.Add(SessionStore.CurrentUserKey, username);
            request.Session.Add(ShoppingCart.SessionKey, new ShoppingCart());
        }
    }
}
