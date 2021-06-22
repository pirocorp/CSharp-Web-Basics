namespace CarShop.Services
{
    using System.Linq;
    using Data;

    public class UserService : IUserService
    {
        private readonly CarShopDbContext dbContext;

        public UserService(CarShopDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool UserIsMechanic(string userId)
            => this.dbContext.Users
                .Any(u => u.Id == userId && u.IsMechanic);
    }
}