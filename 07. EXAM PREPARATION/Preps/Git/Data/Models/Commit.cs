namespace Git.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class Commit
    {
        public Commit()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreateOn = DateTime.UtcNow;
        }

        [Key]
        [Required]
        [StringLength(IdMaxLength)]
        public string Id { get; init; }

        [Required]
        public string Description { get; set; }

        public DateTime CreateOn { get; init; }

        [Required]
        public string CreatorId { get; set; }

        public User Creator { get; set; }

        [Required]
        public string RepositoryId { get; set; }

        public Repository Repository { get; set; }
    }
}
