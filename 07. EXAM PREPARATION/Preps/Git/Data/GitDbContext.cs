namespace Git.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class GitDbContext : DbContext
    {
        public DbSet<User> Users { get; init; }

        public DbSet<Repository> Repositories { get; set; }

        public DbSet<Commit> Commits { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS2019;Database=GitDb;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }
    }
}
