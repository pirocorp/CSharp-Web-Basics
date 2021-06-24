namespace BattleCards.Services
{
    using System.Collections.Generic;
    using Models.Cards;
    using Models.Users;

    public interface IValidator
    {
        ICollection<string> ValidateUser(RegisterUserFormModel model);

        ICollection<string> ValidateCardCreation(CardCreateFormModel model);
    }
}
