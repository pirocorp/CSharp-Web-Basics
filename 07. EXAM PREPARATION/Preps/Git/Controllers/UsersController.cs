namespace Git.Controllers
{
    using System.Linq;

    using Data;
    using Data.Models;
    using Models.Users;
    using MyWebServer.Controllers;
    using MyWebServer.Http;
    using Services;

    public class UsersController : Controller
    {
        private readonly IValidator validator;
        private readonly IPasswordHasher passwordHasher;
        private readonly GitDbContext dbContext;

        public UsersController(
            IValidator validator,
            IPasswordHasher passwordHasher,
            GitDbContext dbContext)
        {
            this.validator = validator;
            this.passwordHasher = passwordHasher;
            this.dbContext = dbContext;
        }

        public HttpResponse Login() => this.View();

        [HttpPost]
        public HttpResponse Login(LoginUserFormModel model)
        {
            var hashedPassword = this.passwordHasher.HashPassword(model.Password);

            var userId = this.dbContext.Users
                .Where(u => u.Username == model.Username && u.Password == hashedPassword)
                .Select(u => u.Id)
                .FirstOrDefault();

            if (userId is null)
            {
                return this.Error("Invalid username or password.");
            }

            this.SignIn(userId);

            return this.Redirect("/Repositories/All");
        }

        public HttpResponse Register() => this.View();

        [HttpPost]
        public HttpResponse Register(RegisterUserFormModel model)
        {
            var modelErrors = this.validator.ValidateUser(model);

            if (this.dbContext.Users.Any(u => u.Username == model.Username))
            {
                modelErrors.Add($"Username {model.Username} already exists.");
            }

            if (this.dbContext.Users.Any(u => u.Email == model.Email))
            {
                modelErrors.Add($"Email {model.Email} already exists.");
            }

            if (modelErrors.Any())
            {
                return this.Error(modelErrors);
            }

            var user = new User()
            {
                Username = model.Username,
                Password = this.passwordHasher.HashPassword(model.Password),
                Email = model.Email,
            };

            this.dbContext.Add(user);
            this.dbContext.SaveChanges();

            return this.Redirect("/Users/Login");
        }

        public HttpResponse Logout()
        {
            this.SignOut();

            return this.Redirect("/");
        }
    }
}
