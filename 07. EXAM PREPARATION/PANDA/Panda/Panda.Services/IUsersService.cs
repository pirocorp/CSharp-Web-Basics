namespace Panda.Services
{
    using System.Collections.Generic;
    using Models;

    public interface IUsersService
    {
        string CreateUser(string username, string email, string password);

        User GetUserOrNull(string username, string password);

        IEnumerable<string> GetUsernames();
    }
}
