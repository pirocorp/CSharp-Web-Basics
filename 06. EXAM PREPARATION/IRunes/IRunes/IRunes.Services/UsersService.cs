namespace IRunes.Services
{
    using System.Security.Cryptography;
    using System.Text;
    using Data;

    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext _db;

        public UsersService(ApplicationDbContext db)
        {
            this._db = db;
        }


        public string GetUserId(string username, string password)
        {
            throw new System.NotImplementedException();
        }

        public void Register(string username, string email, string password)
        {
            throw new System.NotImplementedException();
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
