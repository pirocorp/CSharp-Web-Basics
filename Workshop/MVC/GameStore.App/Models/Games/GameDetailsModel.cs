namespace GameStore.App.Models.Games
{
    using System;
    using Data.Models;
    using Infrastructure.Mapping;

    public class GameDetailsModel : IMapFrom<Game>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public double Size { get; set; }

        public string VideoId { get; set; }

        public string ThumbnailUrl { get; set; }

        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}
