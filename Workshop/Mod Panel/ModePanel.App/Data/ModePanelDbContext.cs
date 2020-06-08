namespace ModePanel.App.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class ModePanelDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(@"Server=PIRO\SQLEXPRESS2019;Database=ModePanelDb;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
