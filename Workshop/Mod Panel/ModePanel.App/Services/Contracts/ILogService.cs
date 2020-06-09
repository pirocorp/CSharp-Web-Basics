namespace ModePanel.App.Services.Contracts
{
    using System.Collections.Generic;
    using Data.Models;
    using Models.Admin;
    using Models.Logs;

    public interface ILogService
    {
        void Create(string admin, LogType type, string additionalInfo);

        IEnumerable<LogModel> All();
    }
}
