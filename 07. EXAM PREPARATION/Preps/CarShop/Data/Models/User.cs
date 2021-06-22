namespace CarShop.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class User
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        [Required]
        [StringLength(IdMaxLength)]
        public string Id { get; init; }

        [Required]
        [StringLength(DefaultMaxLength, MinimumLength = DefaultMinLength)]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool IsMechanic { get; set; }
    }
}
