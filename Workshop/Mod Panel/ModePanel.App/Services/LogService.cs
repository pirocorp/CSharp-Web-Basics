namespace ModePanel.App.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Data.Models;
    using Models.Logs;

    public class LogService : ILogService
    {
        private readonly ModePanelDbContext _db;
        private readonly IMapper _mapper;

        public LogService(ModePanelDbContext db, IMapper mapper)
        {
            this._db = db;
            this._mapper = mapper;
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
                .ProjectTo<LogModel>(this._mapper.ConfigurationProvider)
                .ToList();
    }
}
