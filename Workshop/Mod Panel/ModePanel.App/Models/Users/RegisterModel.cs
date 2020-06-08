﻿namespace ModePanel.App.Models.Users
{
    using Infrastructure;
    using Infrastructure.Validation.Users;

    public class RegisterModel
    {
        [Email]
        [Required]
        public string Email { get; set; }

        public string FullName { get; set; }

        [Required]
        [Password]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}
