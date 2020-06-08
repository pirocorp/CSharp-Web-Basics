namespace ModePanel.App.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Contracts;
    using Data;
    using Data.Models;
    using Models.Admin;

    public class UserService : IUserService
    {
        public bool Create(string email, string password, PositionType position)
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
                    Position = position,
                    IsAdmin = isAdmin,
                    IsApproved = isAdmin
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

        public bool UserIsApproved(string email)
        {
            using var db = new ModePanelDbContext();

            return db.Users
                .Any(u => u.Email == email && u.IsApproved);
        }

        public IEnumerable<AdminUserModel> All()
        {
            using (var db = new ModePanelDbContext())
            {
                return db.Users
                    .Select(u => new AdminUserModel
                    {
                        Id = u.Id,
                        Email = u.Email,
                        Position = u.Position,
                        IsApproved = u.IsApproved,
                        Posts = 0 // TODO: Implement
                    })
                    .ToList();
            }
        }

        public void Approve(int id)
        {
            using (var db = new ModePanelDbContext())
            {
                var user = db.Users.Find(id);

                if (user != null)
                {
                    user.IsApproved = true;
                    db.SaveChanges();
                }
            }
        }
    }
}
