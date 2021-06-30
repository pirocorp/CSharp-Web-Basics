namespace SharedTrip.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    // I know that structs and primitives do not need required if they are not nullable
    // but i add required attribute for consistency
    public class Trip
    {
        public Trip()
        {
            this.Id = Guid.NewGuid().ToString();
            this.UserTrips = new List<UserTrip>();
        }

        [Key]
        [Required]
        [StringLength(IdMaxLength)]
        public string Id { get; init; }

        [Required]
        public string StartPoint { get; init; }

        [Required]
        public string EndPoint { get; init; }

        [Required]
        public DateTime DepartureTime { get; init; }

        [Required]
        [Range(SeatsMinValue, SeatsMaxValue)]
        public int Seats { get; init; }

        [Required]
        [StringLength(DescriptionMaxValue)]
        public string Description { get; init; }

        public string ImagePath { get; init; }

        public IEnumerable<UserTrip> UserTrips { get; init; }
    }
}
