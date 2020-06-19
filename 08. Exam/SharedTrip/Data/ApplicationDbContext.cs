namespace SharedTrip
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class ApplicationDbContext : DbContext
    { 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(DatabaseConfiguration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<UserTrip>()
                .HasKey(ut => new {ut.TripId, ut.UserId});

            modelBuilder
                .Entity<UserTrip>()
                .HasOne(ut => ut.Trip)
                .WithMany(t => t.UsersTrips)
                .HasForeignKey(ut => ut.TripId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<UserTrip>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UsersTrips)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
