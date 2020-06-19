namespace SharedTrip.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();
            this.UsersTrips = new HashSet<UserTrip>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }

        public ICollection<UserTrip> UsersTrips { get; set; }
    }
}
