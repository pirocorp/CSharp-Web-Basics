namespace IRunes.Services
{
    using System.Collections.Generic;
    using Models.Albums;

    public interface IAlbumsService
    {
        void Create(string name, string cover);

        IEnumerable<AlbumInfoModel> GetAll();

        AlbumDetailsModel GetDetails(string id);
    }
}
