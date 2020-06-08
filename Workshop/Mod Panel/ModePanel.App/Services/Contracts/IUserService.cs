namespace ModePanel.App.Services.Contracts
{
    using Data.Models;

    public interface IUserService
    {
        bool Create(string email, string password, PositionType position);

        bool Exists(string email, string password);
    }
}
