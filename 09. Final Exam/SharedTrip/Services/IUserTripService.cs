namespace SharedTrip.Services
{
    using System.Linq;
    using Data;
    using Data.Models;

    public interface IUserTripService
    {
        bool UserIsRegisteredForTrip(string userId, string tripId);

        void AddUserToTrip(string userId, string tripId);
    }

    public class UserTripService : IUserTripService
    {
        private readonly ApplicationDbContext dbContext;

        public UserTripService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool UserIsRegisteredForTrip(string userId, string tripId)
            => this.dbContext.UsersTrips.Any(ut => ut.UserId == userId && ut.TripId == tripId);

        public void AddUserToTrip(string userId, string tripId)
        {
            var userTrip = new UserTrip()
            {
                UserId = userId,
                TripId = tripId
            };

            this.dbContext.Add(userTrip);
            this.dbContext.SaveChanges();
        }
    }
}
