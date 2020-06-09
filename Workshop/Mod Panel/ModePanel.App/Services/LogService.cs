namespace ModePanel.App.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Data;
    using Data.Models;
    using Models.Logs;

    public class LogService : ILogService
    {
        private readonly ModePanelDbContext _db;

        public LogService(ModePanelDbContext db)
        {
            this._db = db;
        }

        public void Create(string admin, LogType type, string additionalInfo)
        {
            var log = new Log()
            {
                Admin = admin,
                Type = type,
                AdditionalInformation = additionalInfo
            };

            this._db.Logs.Add(log);
            this._db.SaveChanges();
        }

        public IEnumerable<LogModel> All()
            => this._db.Logs
                .OrderByDescending(l => l.Id)
                .Select(l => new LogModel()
                {
                    Admin = l.Admin,
                    Type = l.Type,
                    AdditionalInformation = l.AdditionalInformation
                })
                .ToList();
    }
}
