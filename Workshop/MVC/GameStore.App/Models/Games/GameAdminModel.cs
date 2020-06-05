namespace GameStore.App.Models.Games
{
    using System;
    using Infrastructure.Validation;
    using Infrastructure.Validation.Games;
    using SimpleMvc.Framework.Attributes.Validation;

    public class GameAdminModel
    {
        [Title]
        [Required]
        public string Title { get; set; }

        [Required]
        [Description]
        public string Description { get; set; }

        [ThumbnailUrl]
        public string ThumbnailUrl { get; set; }

        [NumberRange(0, double.MaxValue)]
        public decimal Price { get; set; }

        [NumberRange(0, double.MaxValue)]
        public double Size { get; set; }

        [VideoId]
        [Required]
        public string VideoId { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}
