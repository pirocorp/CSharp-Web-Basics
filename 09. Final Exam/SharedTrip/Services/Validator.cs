namespace SharedTrip.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using Models.Trips;
    using Models.Users;

    using static Data.DataConstants;

    public class Validator : IValidator
    {
        public ICollection<string> ValidateUser(RegisterUserFormModel model)
        {
            var errors = new List<string>();

            if (model.Username.Length is < UserMinUsername or > UserMaxUsername)
            {
                errors.Add($"Username '{model.Username}' must be between {UserMinUsername} and {UserMaxUsername} characters long.");
            }

            if (model.Password.Length is < UserMinPassword or > UserMaxPassword)
            {
                errors.Add($"Password must be between {UserMinPassword} and {UserMaxPassword} characters long.");
            }

            if (model.Password != model.ConfirmPassword)
            {
                errors.Add($"Passwords did not match.");
            }

            if (!Regex.IsMatch(model.Email, UserEmailRegularExpression))
            {
                errors.Add($"Email {model.Email} is not valid email.");
            }

            return errors;
        }

        public ICollection<string> ValidateCreateTrip(TripCreateFormModel model)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(model.StartPoint))
            {
                errors.Add($"Start point cannot be empty string.");
            }

            if (string.IsNullOrWhiteSpace(model.EndPoint))
            {
                errors.Add($"End point cannot be empty string.");
            }

            if (model.Seats is < SeatsMinValue or > SeatsMaxValue)
            {
                errors.Add($"Seats must be between {SeatsMinValue} and {SeatsMaxValue}");
            }

            if (model.Description.Length > DescriptionMaxValue)
            {
                errors.Add($"Description must be not more than {DescriptionMaxValue} characters long.");
            }

            if (!Uri.IsWellFormedUriString(model.ImagePath, UriKind.Absolute))
            {
                errors.Add($"Invalid image path.");
            }

            return errors;
        }
    }
}
