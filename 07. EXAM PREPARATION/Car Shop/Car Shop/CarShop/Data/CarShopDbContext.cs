namespace CarShop.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class CarShopDbContext : DbContext
    {
        public DbSet<User> Users { get; init; }

        public DbSet<Car> Cars { get; init; }

        public DbSet<Issue> Issues { get; init; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS2019;Database=CarShopDb;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }
    }
}
