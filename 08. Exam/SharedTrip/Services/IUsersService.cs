namespace SharedTrip.Services
{
    using Models;

    public interface IUsersService
    {
        public User GetUserOrDefault(string username, string password);

        bool UsernameExists(string inputUsername);

        bool EmailExists(string inputEmail);

        void Create(string username, string email, string password);
    }
}
