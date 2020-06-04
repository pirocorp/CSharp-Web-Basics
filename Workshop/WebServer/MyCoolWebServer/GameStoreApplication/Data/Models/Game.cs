namespace MyCoolWebServer.GameStoreApplication.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common;

    public class Game
    {
        public Game()
        {
            this.Users = new HashSet<UserGame>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(ValidationConstants.Game.TitleMinLength)]
        [MaxLength(ValidationConstants.Game.TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MinLength(ValidationConstants.Game.VideoLength)]
        [MaxLength(ValidationConstants.Game.VideoLength)]
        public string VideoId { get; set; }

        [Required]
        public string Image { get; set; }

        /// <summary>
        /// Size is in GB
        /// </summary>
        public double Size { get; set; }

        public decimal Price { get; set; }

        [Required]
        [MinLength(ValidationConstants.Game.DescriptionMinLength)]
        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }

        public ICollection<UserGame> Users { get; set; }
    }
}
