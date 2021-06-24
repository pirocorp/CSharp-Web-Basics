namespace BattleCards.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Models.Cards;
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

        public ICollection<string> ValidateCardCreation(CardCreateFormModel model)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(model.Name) || model.Name.Length is < CardMinName or > CardMaxName)
            {
                errors.Add($"{nameof(model.Name)} '{model.Name}' must be between {CardMinName} and {CardMaxName} characters long.");
            }

            if (string.IsNullOrWhiteSpace(model.Description) || model.Description.Length > CardMaxDescription)
            {
                errors.Add($"{nameof(model.Description)} '{model.Description}' no more than {CardMaxDescription} characters long.");
            }

            if (string.IsNullOrWhiteSpace(model.Image) || !Uri.IsWellFormedUriString(model.Image, UriKind.Absolute))
            {
                errors.Add($"{nameof(model.Image)} '{model.Image}' is invalid uri.");
            }

            if (KeywordValues.All(k => k != model.Keyword))
            {
                errors.Add($"{nameof(model.Keyword)} '{model.Keyword}' is invalid.");
            }

            if (model.Attack < 0)
            {
                errors.Add($"{nameof(model.Attack)} '{model.Attack}' cannot be negative.");
            }

            if (model.Health < 0)
            {
                errors.Add($"{nameof(model.Health)} '{model.Health}' cannot be negative.");
            }

            return errors;
        }
    }
}
