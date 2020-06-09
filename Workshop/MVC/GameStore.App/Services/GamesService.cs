namespace GameStore.App.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Data;
    using Data.Models;
    using Models.Games;

    public class GamesService : IGamesService
    {
        private readonly GameStoreDbContext _db;

        public GamesService(GameStoreDbContext db)
        {
            this._db = db;
        }

        public IEnumerable<GameListingAdminModel> All()
            => this._db.Games
                .Select(g => new GameListingAdminModel()
                {
                    Id = g.Id,
                    Name = g.Title,
                    Price = g.Price,
                    Size = g.Size
                })
                .ToList();

        public void Create(string title, string description, string thumbnailUrl, 
            decimal price, double size, string videoId, DateTime releaseDate)
        {
            var game = new Game()
            {
                Title = title,
                Description = description,
                ThumbnailUrl = thumbnailUrl,
                Price = price,
                Size = size,
                VideoId = videoId,
                ReleaseDate = releaseDate
            };

            this._db.Games.Add(game);
            this._db.SaveChanges();
        }

        public Game GetById(int id)
            => this._db.Games.Find(id);

        public void Update(int id, string modelTitle, string modelDescription, 
            string modelThumbnailUrl, decimal modelPrice, double modelSize, 
            string modelVideoId, DateTime modelReleaseDate)
        {
            var game = this._db.Games.Find(id);

            game.Title = modelTitle;
            game.Description = modelDescription;
            game.ThumbnailUrl = modelThumbnailUrl;
            game.Price = modelPrice;
            game.Size = modelSize;
            game.VideoId = modelVideoId;
            game.ReleaseDate = modelReleaseDate;

            this._db.SaveChanges();
        }

        public void Delete(int id)
        {
            var game = this._db.Games.Find(id);

            this._db.Games.Remove(game);
            this._db.SaveChanges();
        }
    }
}
