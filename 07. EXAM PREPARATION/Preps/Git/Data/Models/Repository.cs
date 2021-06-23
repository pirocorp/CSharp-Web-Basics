namespace Git.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class Repository
    {
        public Repository()
        {
            this.CreateOn = DateTime.UtcNow;
            this.Commits = new List<Commit>();
        }

        [Key]
        [Required]
        [StringLength(IdMaxLength)]
        public string Id { get; init; }

        [Required]
        [StringLength(RepositoryMaxName)]
        public string Name { get; set; }

        public DateTime CreateOn { get; init; }

        public bool IsPublic { get; set; }

        [Required]
        public string OwnerId { get; set; }

        public User Owner { get; set; }

        public IEnumerable<Commit> Commits { get; init; }
    }
}
