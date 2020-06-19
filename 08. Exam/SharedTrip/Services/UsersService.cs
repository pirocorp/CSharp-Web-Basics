namespace SharedTrip.Services
{
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Models;

    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext _db;

        public UsersService(ApplicationDbContext db)
        {
            this._db = db;
        }

        public User GetUserOrDefault(string username, string password)
        {
            var passwordHash = this.Hash(password);

            return this._db
                .Users
                .SingleOrDefault(u => u.Username == username 
                                   && u.Password == passwordHash);
        }

        public bool UsernameExists(string username)
            => this._db.Users
                    .Any(u => u.Username == username);

        public bool EmailExists(string email)
            => this._db.Users
                    .Any(u => u.Email == email);

        public void Create(string username, string email, string password)
        {
            var user = new User()
            {
                Username = username,
                Email = email,
                Password = this.Hash(password),
            };

            this._db.Users.Add(user);
            this._db.SaveChanges();
        }

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
