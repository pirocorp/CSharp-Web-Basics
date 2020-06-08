namespace ModePanel.App.Services
{
    using System.Linq;

    using Contracts;
    using Data;
    using Data.Models;

    public class UserService : IUserService
    {
        public bool Create(string email, string password)
        {
            using (var db = new ModePanelDbContext())
            {
                if (db.Users.AsQueryable().Any(u => u.Email == email))
                {
                    return false; 
                }

                var isAdmin = !db.Users.Any();

                var user = new User()
                {
                    Email = email,
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
            using var db = new ModePanelDbContext();

            return db.Users
                .Any(u => u.Email == email && u.Password == password);
        }
    }
}
