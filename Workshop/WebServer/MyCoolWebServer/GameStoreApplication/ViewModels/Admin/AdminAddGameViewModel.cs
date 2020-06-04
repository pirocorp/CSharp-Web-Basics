namespace MyCoolWebServer.GameStoreApplication.ViewModels.Admin
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Common;

    public class AdminAddGameViewModel
    {
        [Required]
        [MinLength(
            ValidationConstants.Game.TitleMinLength,
            ErrorMessage = ValidationConstants.InvalidMinLengthErrorMessage)]
        [MaxLength(
            ValidationConstants.Game.TitleMaxLength, 
            ErrorMessage = ValidationConstants.InvalidMaxLengthErrorMessage)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "YouTube Video URL")]
        [MinLength(
            ValidationConstants.Game.VideoLength,
            ErrorMessage = ValidationConstants.ExactLengthErrorMessage)]
        [MaxLength(
            ValidationConstants.Game.VideoLength, 
            ErrorMessage = ValidationConstants.ExactLengthErrorMessage)]
        public string VideoId { get; set; }

        [Required]
        public string Image { get; set; }

        /// <summary>
        /// Size is in GB
        /// </summary>
        public double Size { get; set; }

        public decimal Price { get; set; }

        [Required]
        [MinLength(
            ValidationConstants.Game.DescriptionMinLength,
            ErrorMessage = ValidationConstants.InvalidMinLengthErrorMessage)]
        public string Description { get; set; }

        [Display(Name = "Release Date")]
        [Required]
        public DateTime? ReleaseDate { get; set; }
    }
}
