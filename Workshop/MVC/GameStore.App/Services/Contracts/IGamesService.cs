namespace GameStore.App.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using Data.Models;

    public interface IGamesService
    {
        bool Exists(int id);

        IEnumerable<TModel> All<TModel>(int? userId = null);

        IEnumerable<TModel> ByIds<TModel>(IEnumerable<int> ids);

        void Create(string title, string description, string thumbnailUrl, decimal price, double size, string videoId, DateTime releaseDate);

        Game GetById(int id);

        void Update(int id, string modelTitle, string modelDescription, string modelThumbnailUrl, decimal modelPrice, double modelSize, string modelVideoId, DateTime modelReleaseDate);

        void Delete(int id);
    }
}
