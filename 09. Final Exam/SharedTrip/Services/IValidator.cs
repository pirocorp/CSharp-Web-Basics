namespace SharedTrip.Services
{
    using System.Collections.Generic;
    using Models.Trips;
    using Models.Users;

    public interface IValidator
    {
        ICollection<string> ValidateUser(RegisterUserFormModel model);

        ICollection<string> ValidateCreateTrip(TripCreateFormModel model);
    }
}
