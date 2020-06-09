namespace ModePanel.App.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Data;
    using Data.Models;
    using Infrastructure;
    using Models.Home;
    using Models.Posts;

    public class PostService : IPostService
    {
        public void Create(string title, string content, int userId)
        {
            using var db =  new ModePanelDbContext();
            {
                var post = new Post()
                {
                    Title = title.Capitalize(),
                    Content = content,
                    UserId = userId,
                    CreatedOn = DateTime.UtcNow
                };

                db.Posts.Add(post);
                db.SaveChanges();
            }
        }

        public IEnumerable<PostListingModel> All()
        {
            using var db = new ModePanelDbContext();

            return db.Posts
                .Select(p => new PostListingModel
                {
                    Id = p.Id,
                    Title = p.Title
                })
                .ToList();
        }

        public IEnumerable<HomeListingModel> All(int id)
        {
            using var db = new ModePanelDbContext();

            return db.Posts
                .Where(p => p.Id == id)
                .OrderByDescending(p => p.CreatedOn)
                .Select(p => new HomeListingModel
                {
                    Title = p.Title,
                    Content = p.Content,
                    CreatedBy = p.User.Email,
                    CreatedOn = p.CreatedOn
                })
                .ToList();
        }

        public IEnumerable<HomeListingModel> AllWithData(string search = null)
        {
            using var db = new ModePanelDbContext();

            var query = db.Posts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query
                    .Where(p => p.Title.ToLower().Contains(search.ToLower()));
            }

            return query
                .OrderByDescending(p => p.Id)
                .Select(p => new HomeListingModel
                {
                    Title = p.Title,
                    Content = p.Content,
                    CreatedBy = p.User.Email,
                    CreatedOn = p.CreatedOn
                })
                .ToList();
        }

        public PostModel GetById(int id)
        {
            using (var db = new ModePanelDbContext())
            {
                return db.Posts
                    .Where(p=> p.Id == id)
                    .Select(p => new PostModel
                    {
                        Title = p.Title,
                        Content = p.Content
                    })
                    .FirstOrDefault();
            }
        }

        public void Update(int id, string title, string content)
        {
            using (var db = new ModePanelDbContext())
            {
                var post = db.Posts.Find(id);

                if (post == null)
                {
                    return;
                }

                post.Title = title.Capitalize();
                post.Content = content;

                db.SaveChanges();
            }
        }

        public string Delete(int id)
        {
            using (var db = new ModePanelDbContext())
            {
                var post = db.Posts.Find(id);

                if (post == null)
                {
                    return null;
                }

                db.Posts.Remove(post);
                db.SaveChanges();

                return post.Title;
            }
        }
    }
}
