namespace ModePanel.App.Services.Contracts
{
    public interface IUserService
    {
        bool Create(string email, string password);

        bool Exists(string email, string password);
    }
}
