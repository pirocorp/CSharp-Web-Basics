namespace ModePanel.App.Services.Contracts
{
    using System.Collections.Generic;
    using Data.Models;
    using Models.Admin;

    public interface ILogService
    {
        void Create(string admin, LogType type, string additionalInfo);

        IEnumerable<AdminLogModel> All();
    }
}
