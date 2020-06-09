namespace GameStore.App.Services
{
    using System.Linq;
    using Contracts;
    using Data;
    using Data.Models;

    public class UsersService : IUsersService
    {
        private readonly GameStoreDbContext _db;

        public UsersService(GameStoreDbContext db)
        {
            this._db = db;
        }

        public bool Create(string email, string password, string name)
        {
            if (this._db.Users.AsQueryable().Any(u => u.Email == email))
            {
                return false; 
            }

            var isAdmin = !this._db.Users.Any();

            var user = new User()
            {
                Email = email,
                Name = name,
                Password = password,
                IsAdmin = isAdmin
            };

            this._db.Users.Add(user);
            this._db.SaveChanges();

            return true;
        }

        public bool Exists(string email, string password)
            => this._db.Users
                .Any(u => u.Email == email && u.Password == password);
    }
}
