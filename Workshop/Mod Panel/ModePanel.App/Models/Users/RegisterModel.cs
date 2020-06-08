namespace ModePanel.App.Models.Users
{
    using Data.Models;
    using Infrastructure;
    using Infrastructure.Validation.Users;

    public class RegisterModel
    {
        [Email]
        [Required]
        public string Email { get; set; }

        [Required]
        [Password]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        public string Position { get; set; }

        public PositionType PositionType () => (PositionType) (int.Parse(this.Position));
    }
}
