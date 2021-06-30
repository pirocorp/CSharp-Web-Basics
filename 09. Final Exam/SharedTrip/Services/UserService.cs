namespace SharedTrip.Services
{
    using System.Linq;

    using SharedTrip.Data;
    using SharedTrip.Data.Models;

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IPasswordHasher passwordHasher;

        public UserService(ApplicationDbContext dbContext, IPasswordHasher passwordHasher)
        {
            this.dbContext = dbContext;
            this.passwordHasher = passwordHasher;
        }

        public string ValidateUserCredentials(string username, string password)
        {
            var hashedPassword = this.passwordHasher.HashPassword(password);

            var userId = this.dbContext.Users
                .Where(u => u.Username == username && u.Password == hashedPassword)
                .Select(u => u.Id)
                .FirstOrDefault();

            return userId;
        }

        public bool UsernameAlreadyExists(string username)
            => this.dbContext.Users.Any(u => u.Username == username);

        public bool EmailAlreadyExists(string email)
            => this.dbContext.Users.Any(u => u.Email == email);

        public void RegisterUser(
            string username, 
            string email, 
            string password)
        {
            var user = new User()
            {
                Username = username,
                Password = this.passwordHasher.HashPassword(password),
                Email = email,
            };

            this.dbContext.Add(user);
            this.dbContext.SaveChanges();
        }
    }
}
