namespace CarShop.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Models.Cars;
    using Models.Users;

    using static Data.DataConstants;

    public class Validator : IValidator
    {
        public ICollection<string> ValidateUserRegistration(RegisterUserFormModel model)
        {
            var errors = new List<string>();

            if (model.Username.Length is < DefaultMinLength or > DefaultMaxLength)
            {
                errors.Add($"Username '{model.Username}' must be between {DefaultMinLength} and {DefaultMaxLength} characters long.");
            }

            if (model.Password.Length is < DefaultMinLength or > DefaultMaxLength)
            {
                errors.Add($"Password must be between {DefaultMinLength} and {DefaultMaxLength} characters long.");
            }

            if (model.Password != model.ConfirmPassword)
            {
                errors.Add($"Passwords did not match.");
            }

            if (!model.Email.Contains("@"))
            {
                errors.Add($"Email {model.Email} is not valid email.");
            }

            if (model.UserType != Mechanic && model.UserType != Client)
            {
                errors.Add($"User type '{model.UserType}' is not valid user type.");
            }

            return errors;
        }

        public ICollection<string> ValidateCarCreation(AddCarFormModel model)
        {
            var errors = new List<string>();

            if (model.Model.Length is < DefaultMinLength or > DefaultMaxLength)
            {
                errors.Add($"Model '{model.Model}' must be between {DefaultMinLength} and {DefaultMaxLength} characters long.");
            }

            var carYearMaxValue = DateTime.Now.Year + CarYearMaxValueOffset;

            if (model.Year < CarYearMinValue || model.Year > carYearMaxValue)
            {
                errors.Add($"Year '{model.Year}' must be between {CarYearMinValue} and {carYearMaxValue} characters long.");
            }

            if (!Regex.IsMatch(model.PlateNumber, CarPlateNumberRegularExpression))
            {
                errors.Add($"Plate number {model.PlateNumber} is not valid plate number. It should be in format 'AA0000AA'");
            }

            return errors;
        }
    }
}
