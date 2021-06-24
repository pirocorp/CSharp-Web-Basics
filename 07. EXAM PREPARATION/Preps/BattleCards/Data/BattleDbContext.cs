namespace BattleCards.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class BattleDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<UserCard> UsersCards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS2019;Database=BattleCardsDb;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCard>()
                .HasKey(k => new { k.UserId, k.CardId });

            modelBuilder.Entity<UserCard>()
                .HasIndex(uc => uc.CardId);

            modelBuilder.Entity<UserCard>()
                .HasIndex(uc => uc.UserId);
        }
    }
}
