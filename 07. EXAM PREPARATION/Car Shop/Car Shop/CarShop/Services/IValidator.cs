namespace CarShop.Services
{
    using System.Collections.Generic;
    using Models.Cars;
    using Models.Users;

    public interface IValidator
    {
        ICollection<string> ValidateUserRegistration(RegisterUserFormModel model);

        ICollection<string> ValidateCarCreation(AddCarFormModel model);
    }
}
