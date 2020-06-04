namespace MyCoolWebServer.GameStoreApplication.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Data;
    using Data.Models;
    using ViewModels.Admin;

    public class GameService : IGameService
    {
        public void Create(string title, string description, string image, 
            decimal price, double size, string videoId, DateTime releaseDate)
        {
            using (var db = new GameStoreDbContext())
            {
                var game = new Game()
                {
                    Title = title,
                    Description = description,
                    Image = image,
                    Price = price,
                    Size = size,
                    VideoId = videoId,
                    ReleaseDate = releaseDate
                };

                db.Games.Add(game);
                db.SaveChanges();
            }
        }

        public IEnumerable<AdminListGameViewModel> All()
        {
            using (var db = new GameStoreDbContext())
            {
                return db.Games
                    .Select(g => new AdminListGameViewModel()
                    {
                        Id = g.Id,
                        Title = g.Title,
                        Price = g.Price,
                        Size = g.Size
                    })
                    .ToArray();
            }
        }
    }
}
