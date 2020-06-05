namespace GameStore.App.Services
{
    using System.Linq;
    using Contracts;
    using Data;
    using Data.Models;

    public class UserService : IUserService
    {
        public bool Create(string email, string password, string name)
        {
            using (var db = new GameStoreDbContext())
            {
                if (db.Users.AsQueryable().Any(u => u.Email == email))
                {
                    return false; 
                }

                var isAdmin = !db.Users.Any();

                var user = new User()
                {
                    Email = email,
                    Name = name,
                    Password = password,
                    IsAdmin = isAdmin
                };

                db.Users.Add(user);
                db.SaveChanges();
            }

            return true;
        }

        public bool Exists(string email, string password)
        {
            using var db = new GameStoreDbContext();

            return db.Users
                .Any(u => u.Email == email && u.Password == password);
        }
    }
}
