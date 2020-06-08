namespace ModePanel.App.Services.Contracts
{
    using System.Collections.Generic;
    using Data.Models;
    using Models.Admin;

    public interface IUserService
    {
        bool Create(string email, string password, PositionType position);

        bool Exists(string email, string password);

        bool UserIsApproved(string modelEmail);

        IEnumerable<AdminUserModel> All();

        void Approve(int id);
    }
}
