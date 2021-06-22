namespace Git.Data
{
    using Microsoft.EntityFrameworkCore;

    public class GitDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS2019;Database=GitDb;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }
    }
}
