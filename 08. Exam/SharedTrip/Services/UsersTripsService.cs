namespace SharedTrip.Services
{
    using System.Linq;
    using Models;


    public class UsersTripsService : IUsersTripsService
    {
        private readonly ApplicationDbContext _db;
        private readonly ITripsService _tripsService;

        public UsersTripsService(ApplicationDbContext db, ITripsService tripsService)
        {
            this._db = db;
            this._tripsService = tripsService;
        }

        public bool UserExistsOnTrip(string user, string tripId)
            => this._db.UsersTrips
                .Any(ut => ut.UserId == user
                        && ut.TripId == tripId);

        public void RegisterUserForTrip(string userId, string tripId)
        {
            var userTrip = new UserTrip()
            {
                UserId = userId,
                TripId = tripId,
            };

            this._tripsService.ReduceSeats(tripId);

            this._db.UsersTrips.Add(userTrip);
            this._db.SaveChanges();
        }
    }
}
