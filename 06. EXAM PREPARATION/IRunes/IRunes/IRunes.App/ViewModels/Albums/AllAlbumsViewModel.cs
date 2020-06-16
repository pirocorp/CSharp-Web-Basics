namespace IRunes.App.ViewModels.Albums
{
    using System.Collections.Generic;
    using Services.Models.Albums;

    public class AllAlbumsViewModel
    {
        public IEnumerable<AlbumInfoModel> Albums { get; set; }
    }
}
