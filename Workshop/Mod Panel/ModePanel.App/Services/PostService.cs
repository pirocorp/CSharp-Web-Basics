namespace ModePanel.App.Services
{
    using System.Globalization;
    using Contracts;
    using Data;
    using Data.Models;

    public class PostService : IPostService
    {
        public void Create(string title, string content, int userId)
        {
            using var db =  new ModePanelDbContext();
            {
                var post = new Post()
                {
                    Title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title),
                    Content = content,
                    UserId = userId
                };

                db.Posts.Add(post);
                db.SaveChanges();
            }
        }
    }
}
