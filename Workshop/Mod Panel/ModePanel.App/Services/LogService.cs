﻿namespace ModePanel.App.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Data;
    using Data.Models;
    using Models.Admin;
    using Models.Logs;

    public class LogService : ILogService
    {
        public void Create(string admin, LogType type, string additionalInfo)
        {
            using (var db = new ModePanelDbContext())
            {
                var log = new Log()
                {
                    Admin = admin,
                    Type = type,
                    AdditionalInformation = additionalInfo
                };

                db.Logs.Add(log);
                db.SaveChanges();
            }
        }

        public IEnumerable<LogModel> All()
        {
            using (var db = new ModePanelDbContext())
            {
                return db.Logs
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
    }
}