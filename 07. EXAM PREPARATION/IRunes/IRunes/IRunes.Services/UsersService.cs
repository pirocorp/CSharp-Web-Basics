namespace IRunes.Services
{
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Data;
    using IRunes.Models;
    using Models;
    using SIS.MvcFramework;

    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext _db;

        public UsersService(ApplicationDbContext db)
        {
            this._db = db;
        }
        
        public string GetUserId(string username, string password)
        {
            var hashedPassword = this.Hash(password);

            return this._db.Users
                .Where(u => u.Username == username 
                         && u.Password == hashedPassword)
                .Select(u => u.Id)
                .FirstOrDefault();
        }

        public void Register(string username, string email, string password)
        {
            var user = new User
            {
                Username = username,
                Email = email,
                Password = this.Hash(password),
                Role = IdentityRole.User,
            };

            this._db.Users.Add(user);
            this._db.SaveChanges();
        }

        public bool UsernameExists(string username)
            => this._db.Users.Any(u => u.Username == username);

        public bool EmailExists(string email)
            => this._db.Users.Any(u => u.Email == email);

        public string GetUsername(string id)
            => this._db.Users
                .Where(u => u.Id == id)
                .Select(u => u.Username)
                .FirstOrDefault();

        private string Hash(string input)
        {
            var crypt = new SHA256Managed();
            var hash = new StringBuilder();
            var crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(input));
            
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2")); //255 => FF
            }

            return hash.ToString();
        }
    }
}
