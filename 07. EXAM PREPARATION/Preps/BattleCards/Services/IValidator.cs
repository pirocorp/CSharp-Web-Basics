namespace BattleCards.Services
{
    using System.Collections.Generic;
    using Models.Users;

    public interface IValidator
    {
        ICollection<string> ValidateUser(RegisterUserFormModel model);
    }
}
