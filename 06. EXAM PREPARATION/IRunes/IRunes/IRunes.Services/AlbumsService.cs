namespace IRunes.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class AlbumsService : IAlbumsService
    {
        private readonly ApplicationDbContext _db;

        public AlbumsService(ApplicationDbContext db)
        {
            this._db = db;
        }

        public void Create(string name, string cover)
        {
            var album = new Album()
            {
                Name = name,
                Cover = cover,
                Price = 0.0M,
            };

            this._db.Albums.Add(album);
            this._db.SaveChanges();
        }

        public IEnumerable<T> GetAll<T>(Func<Album, T> selectFunc)
            => this._db.Albums
                    .Select(selectFunc)
                    .ToList();

        public T GetDetails<T>(string id, Func<Album, T> selectFunc)
            => this._db.Albums
                .Include(a => a.Tracks)
                .Where(x => x.Id == id)
                .AsEnumerable()
                .Select(selectFunc)
                .FirstOrDefault();
    }
}
