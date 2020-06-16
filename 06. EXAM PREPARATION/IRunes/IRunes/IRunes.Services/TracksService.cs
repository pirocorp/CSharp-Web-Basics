namespace IRunes.Services
{
    using System.Linq;
    using Data;
    using IRunes.Models;
    using Models.Tracks;

    public class TracksService : ITracksService
    {
        private readonly ApplicationDbContext _db;

        public TracksService(ApplicationDbContext db)
        {
            this._db = db;
        }

        public void Create(string name, string link, decimal price, string albumId)
        {
            var track = new Track()
            {
                Name = name,
                Link = link,
                Price = price,
                AlbumId = albumId
            };

            this._db.Tracks.Add(track);

            var totalPrice = this._db.Tracks
                .Where(t => t.AlbumId == albumId)
                .Sum(t => t.Price) + price;

            var album = this._db.Albums.Find(albumId);
            album.Price = totalPrice * 0.87M;

            this._db.SaveChanges();
        }

        public TrackDetailsServiceModel GetDetails(string trackId)
            => this._db.Tracks
                .Where(x => x.Id == trackId)
                .Select(x => new TrackDetailsServiceModel()
                {
                    AlbumId = x.AlbumId,
                    Link = x.Link,
                    Name = x.Name,
                    Price = x.Price
                })
                .FirstOrDefault();
    }
}
