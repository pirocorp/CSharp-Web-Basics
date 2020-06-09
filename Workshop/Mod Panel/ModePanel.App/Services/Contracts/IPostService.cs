namespace ModePanel.App.Services.Contracts
{
    using System.Collections.Generic;
    using Models.Home;
    using Models.Posts;

    public interface IPostService
    {
        void Create(string title, string content, int userId);

        IEnumerable<PostListingModel> All();

        IEnumerable<HomeListingModel> All(int id);

        IEnumerable<HomeListingModel> AllWithData(string search = null);

        PostModel GetById(int id);

        void Update(int id, string title, string content);

        string Delete(int id);
    }
}