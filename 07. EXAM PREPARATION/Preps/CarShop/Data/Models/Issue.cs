namespace CarShop.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class Issue
    {
        public Issue()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        [Required]
        [StringLength(IdMaxLength)]
        public string Id { get; init; }

        [Required]
        public string Description { get; set; }

        public bool IsFixed { get; set; }

        [Required]
        public string CarId { get; set; }

        public Car Car { get; set; }
    }
}
