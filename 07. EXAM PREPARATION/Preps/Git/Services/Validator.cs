namespace Git.Services
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Models.Repositories;
    using Models.Users;

    using static Data.DataConstants;

    public class Validator : IValidator
    {
        public ICollection<string> ValidateUser(RegisterUserFormModel model)
        {
            var errors = new List<string>();

            if (model.Username.Length is < UserMinUsername or > DefaultMaxLength)
            {
                errors.Add($"Username '{model.Username}' must be between {UserMinUsername} and {DefaultMaxLength} characters long.");
            }

            if (model.Password.Length is < UserMinPassword or > DefaultMaxLength)
            {
                errors.Add($"Password must be between {UserMinPassword} and {DefaultMaxLength} characters long.");
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

        public ICollection<string> ValidateRepository(RepositoryCreateFormModel model)
        {
            var errors = new List<string>();

            if (model.Name.Length is < RepositoryMinName or > RepositoryMaxName)
            {
                errors.Add($"Repository name '{model.Name}' must be between {RepositoryMinName} and {RepositoryMaxName} characters long.");
            }

            if (model.RepositoryType != RepositoryPublicType && model.RepositoryType != RepositoryPrivateType)
            {
                errors.Add($"Invalid repository type ({RepositoryPublicType}, {RepositoryPrivateType}).");
            }

            return errors;
        }
    }
}
