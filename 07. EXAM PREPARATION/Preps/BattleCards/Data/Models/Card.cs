namespace BattleCards.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class Card
    {
        public Card()
        {
            this.Users = new List<UserCard>();
        }

        [Key]
        [Required]
        public int Id { get; init; }

        [Required]
        [StringLength(CardMaxName, MinimumLength = CardMinName)]
        public string Name { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required] 
        public string Keyword { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Attack { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Health { get; set; }

        [Required]
        [StringLength(CardMaxDescription)]
        public string Description { get; set; }

        public IEnumerable<UserCard> Users { get; set; }
    }
}
