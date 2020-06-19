namespace SharedTrip.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Trip
    {
        public Trip()
        {
            this.Id = Guid.NewGuid().ToString();
            this.UsersTrips = new HashSet<UserTrip>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        public string StartPoint { get; set; }

        [Required]
        public string EndPoint { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public int Seats { get; set; }

        [Required]
        [MaxLength(80)]
        public string Description { get; set; }

        public string ImagePath { get; set; }

        public ICollection<UserTrip> UsersTrips  { get; set; }
    }
}
