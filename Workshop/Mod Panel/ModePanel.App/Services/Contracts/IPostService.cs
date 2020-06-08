namespace ModePanel.App.Services.Contracts
{
    using System.Collections.Generic;
    using Models.Posts;

    public interface IPostService
    {
        void Create(string title, string content, int userId);

        IEnumerable<PostListingModel> All();

        PostModel GetById(int id);

        void Update(int id, string title, string content);
    }
}