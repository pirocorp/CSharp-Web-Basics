namespace IRunes.Services.Models.Albums
{
    using System.Collections.Generic;

    public class AlbumDetailsModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Cover { get; set; }

        public IEnumerable<TrackAlbumDetailsViewModel> Tracks { get; set; }
    }
}
