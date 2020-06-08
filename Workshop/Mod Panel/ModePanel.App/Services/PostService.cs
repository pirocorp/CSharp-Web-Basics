namespace ModePanel.App.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Data;
    using Data.Models;
    using Infrastructure;
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
                    UserId = userId
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

        public void Delete(int id)
        {
            using (var db = new ModePanelDbContext())
            {
                var post = db.Posts.Find(id);

                if (post == null)
                {
                    return;
                }

                db.Posts.Remove(post);
                db.SaveChanges();
            }
        }
    }
}
