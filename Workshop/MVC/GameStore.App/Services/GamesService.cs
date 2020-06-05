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
        public IEnumerable<GameListingAdminModel> All()
        {
            using var db = new GameStoreDbContext();

            return db.Games
                .Select(g => new GameListingAdminModel()
                {
                    Id = g.Id,
                    Name = g.Title,
                    Price = g.Price,
                    Size = g.Size
                })
                .ToList();
        }

        public void Create(string title, string description, string thumbnailUrl, 
            decimal price, double size, string videoId, DateTime releaseDate)
        {
            using (var db = new GameStoreDbContext())
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

                db.Games.Add(game);
                db.SaveChanges();
            }
        }

        public Game GetById(int id)
        {
            using var db = new GameStoreDbContext();

            return db.Games.Find(id);
        }

        public void Update(int id, string modelTitle, string modelDescription, 
            string modelThumbnailUrl, decimal modelPrice, double modelSize, 
            string modelVideoId, DateTime modelReleaseDate)
        {
            using var db = new GameStoreDbContext();

            var game = db.Games.Find(id);

            game.Title = modelTitle;
            game.Description = modelDescription;
            game.ThumbnailUrl = modelThumbnailUrl;
            game.Price = modelPrice;
            game.Size = modelSize;
            game.VideoId = modelVideoId;
            game.ReleaseDate = modelReleaseDate;

            db.SaveChanges();
        }

        public void Delete(int id)
        {
            using (var db = new GameStoreDbContext())
            {
                var game = db.Games.Find(id);

                db.Games.Remove(game);
                db.SaveChanges();
            }
        }
    }
}
