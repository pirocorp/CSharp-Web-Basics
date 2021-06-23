namespace Git.Services
{
    using System.Collections.Generic;
    using Models.Repositories;
    using Models.Users;

    public interface IValidator
    {
        ICollection<string> ValidateUser(RegisterUserFormModel model);

        ICollection<string> ValidateRepository(RepositoryCreateFormModel model);
    }
}
