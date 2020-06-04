namespace MyCoolWebServer.GameStoreApplication.Utilities
{
    using System.Linq;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore.Internal;

    public class PasswordAttribute : ValidationAttribute
    {
        public PasswordAttribute()
        {
            this.ErrorMessage = "Password should be at least 6 symbols long, should contain at least 1 upper case letter, 1 lowercase letter and 1 digit.";
        }

        public override bool IsValid(object value)
        {
            var password = value as string;

            if (password == null)
            {
                return false;
            }

            return password.Any(char.IsUpper)
                && password.Any(char.IsLower)
                && password.Any(char.IsDigit);
        }
    }
}
