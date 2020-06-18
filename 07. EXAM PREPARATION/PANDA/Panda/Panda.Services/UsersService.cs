namespace Panda.Services
{
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Data;
    using Models;
    using SIS.MvcFramework.Attributes.Action;

    public class UsersService : IUsersService
    {
        private readonly PandaDbContext db;

        public UsersService(PandaDbContext db)
        {
            this.db = db;
        }

        public string CreateUser(string username, string email, string password)
        {
            var user = new User()
            {
                Username = username,
                Email = email,
                Password = this.Hash(password)
            };

            this.db.Users.Add(user);
            this.db.SaveChanges();

            return user.Id;
        }

        public User GetUserOrNull(string username, string password)
        {
            var passwordHash = this.Hash(password);

            return this.db
                .Users
                .SingleOrDefault(x => x.Username == username 
                                   && x.Password == passwordHash);
        }

        [NonAction]
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
