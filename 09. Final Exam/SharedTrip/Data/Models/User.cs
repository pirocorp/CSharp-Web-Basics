namespace SharedTrip.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    using static DataConstants;

    public class User
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();
            this.UserTrips = new List<UserTrip>();
        }

        [Key]
        [Required]
        [StringLength(IdMaxLength)]
        public string Id { get; init; }

        [Required]
        [StringLength(UserMaxUsername, MinimumLength = UserMinUsername)]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public IEnumerable<UserTrip> UserTrips { get; init; }
    }
}
