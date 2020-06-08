﻿namespace ModePanel.App.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public User()
        {
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
    }
}