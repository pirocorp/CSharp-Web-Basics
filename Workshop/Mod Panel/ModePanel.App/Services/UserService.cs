namespace ModePanel.App.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Contracts;
    using Data;
    using Data.Models;
    using Models.Admin;

    public class UserService : IUserService
    {
        private readonly ModePanelDbContext _db;

        public UserService(ModePanelDbContext db)
        {
            this._db = db;
        }

        public bool Create(string email, string password, PositionType position)
        {
            if (this._db.Users.AsQueryable().Any(u => u.Email == email))
            {
                return false; 
            }

            var isAdmin = !this._db.Users.Any();

            var user = new User()
            {
                Email = email,
                Password = password,
                Position = position,
                IsAdmin = isAdmin,
                IsApproved = isAdmin
            };

            this._db.Users.Add(user);
            this._db.SaveChanges();

            return true;
        }

        public bool Exists(string email, string password)
            => this._db.Users
                .Any(u => u.Email == email && u.Password == password);

        public bool UserIsApproved(string email)
            => this._db.Users
                .Any(u => u.Email == email && u.IsApproved);

        public IEnumerable<AdminUserModel> All()
            => this._db.Users
                .Select(u => new AdminUserModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    Position = u.Position,
                    IsApproved = u.IsApproved,
                    Posts = u.Posts.Count
                })
                .ToList();

        public string Approve(int id)
        {
            var user = this._db.Users.Find(id);

            if (user != null)
            {
                user.IsApproved = true;
                this._db.SaveChanges();
            }

            return user?.Email;
        }

        public User GetUserProfile(string userIdentity)
            => this._db.Users
                .First(u => u.Email == userIdentity);
    }
}
