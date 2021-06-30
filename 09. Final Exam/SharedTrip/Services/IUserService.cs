namespace SharedTrip.Services
{
    public interface IUserService
    {
        string ValidateUserCredentials(string username, string password);

        bool UsernameAlreadyExists(string username);

        bool EmailAlreadyExists(string email);

        void RegisterUser(string username, string email, string password);
    }
}
