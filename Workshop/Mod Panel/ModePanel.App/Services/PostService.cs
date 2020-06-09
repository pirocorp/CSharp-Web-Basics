namespace ModePanel.App.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Data.Models;
    using Infrastructure;
    using Models.Home;
    using Models.Posts;

    public class PostService : IPostService
    {
        private readonly ModePanelDbContext _db;
        private readonly IMapper _mapper;

        public PostService(ModePanelDbContext db, IMapper mapper)
        {
            this._db = db;
            this._mapper = mapper;
        }

        public void Create(string title, string content, int userId)
        {
            var post = new Post()
            {
                Title = title.Capitalize(),
                Content = content,
                UserId = userId,
                CreatedOn = DateTime.UtcNow
            };

            this._db.Posts.Add(post);
            this._db.SaveChanges();
        }

        public IEnumerable<PostListingModel> All()
            => this._db.Posts
                .ProjectTo<PostListingModel>(this._mapper.ConfigurationProvider)
                .ToList();

        public IEnumerable<HomeListingModel> All(int id)
            => this._db.Posts
                .Where(p => p.Id == id)
                .OrderByDescending(p => p.CreatedOn)
                .ProjectTo<HomeListingModel>(this._mapper.ConfigurationProvider)
                .ToList();

        public IEnumerable<HomeListingModel> AllWithData(string search = null)
        {
            var query = this._db.Posts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query
                    .Where(p => p.Title.ToLower().Contains(search.ToLower()));
            }

            return query
                .OrderByDescending(p => p.Id)
                .ProjectTo<HomeListingModel>(this._mapper.ConfigurationProvider)
                .ToList();
        }

        public PostModel GetById(int id)
            => this._db.Posts
                .Where(p=> p.Id == id)
                .ProjectTo<PostModel>(this._mapper.ConfigurationProvider)
                .FirstOrDefault();

        public void Update(int id, string title, string content)
        {
            var post = this._db.Posts.Find(id);

            if (post == null)
            {
                return;
            }

            post.Title = title.Capitalize();
            post.Content = content;

            this._db.SaveChanges();
        }

        public string Delete(int id)
        {
            var post = this._db.Posts.Find(id);

            if (post == null)
            {
                return null;
            }

            this._db.Posts.Remove(post);
            this._db.SaveChanges();

            return post.Title;
        }
    }
}
