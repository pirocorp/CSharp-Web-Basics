namespace ModePanel.App.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public User()
        {
            this.Posts = new HashSet<Post>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(50)]
        public string Password { get; set; }

        public PositionType Position { get; set; }

        public bool IsApproved { get; set; }

        public bool IsAdmin { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
