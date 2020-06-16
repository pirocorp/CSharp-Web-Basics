namespace IRunes.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using IRunes.Models;
    using Models;
    using Models.Albums;

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

        public IEnumerable<AlbumInfoModel> GetAll()
            => this._db.Albums
                    .Select(a => new AlbumInfoModel()
                    {
                        Id = a.Id,
                        Name = a.Name
                    })
                    .ToList();

        public AlbumDetailsModel GetDetails(string id)
            => this._db.Albums
                    .Where(x => x.Id == id)
                    .Select(x => new AlbumDetailsModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Price = x.Price,
                        Cover = x.Cover,
                        Tracks = x.Tracks
                            .Select(t => new TrackAlbumDetailsViewModel()
                            {
                                Id = t.Id,
                                Name = t.Name
                            })
                    })
                    .FirstOrDefault();
    }
}
