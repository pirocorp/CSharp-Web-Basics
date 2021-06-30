namespace SharedTrip.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Trip> Trips { get; set; }

        public DbSet<UserTrip> UsersTrips { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(DatabaseConfiguration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTrip>()
                .HasKey(ut => new {ut.UserId, ut.TripId});

            modelBuilder.Entity<UserTrip>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserTrips)
                .HasForeignKey(ut => ut.UserId);

            modelBuilder.Entity<UserTrip>()
                .HasOne(ut => ut.Trip)
                .WithMany(t => t.UserTrips)
                .HasForeignKey(ut => ut.TripId);
        }
    }
}
