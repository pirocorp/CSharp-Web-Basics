namespace SharedTrip.Services
{
    public interface IUsersTripsService
    {
        bool UserExistsOnTrip(string user, string tripId);

        void RegisterUserForTrip(string user, string tripId);
    }
}
