namespace GameStore.App.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Data.Models;
    using Models.Games;
    using Models.Home;

    public class GamesService : IGamesService
    {
        private readonly GameStoreDbContext _db;
        private readonly IMapper _mapper;

        public GamesService(GameStoreDbContext db, IMapper mapper)
        {
            this._db = db;
            this._mapper = mapper;
        }

        public bool Exists(int id)
            => this._db.Games.Any(g => g.Id == id);

        public IEnumerable<TModel> ByIds<TModel>(IEnumerable<int> ids)
            => this._db
                .Games
                .Where(g => ids.Contains(g.Id))
                .ProjectTo<TModel>(this._mapper.ConfigurationProvider)
                .ToList();

        public IEnumerable<TModel> All<TModel>(int? userId = null)
        {
            var query = this._db.Games.AsQueryable();

            if (userId.HasValue)
            {
                query = query
                    .Where(g => g.Orders.Any(o => o.UserId == userId));
            }

            return query
                .ProjectTo<TModel>(this._mapper.ConfigurationProvider)
                .ToList();
        }

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
