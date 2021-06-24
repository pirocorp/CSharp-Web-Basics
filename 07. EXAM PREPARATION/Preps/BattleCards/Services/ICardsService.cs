namespace BattleCards.Services
{
    using System.Collections.Generic;
    using Models.Cards;

    public interface ICardsService
    {
        void CreateCard(string name, string imageUrl, string keyword, int attack, int health, string description);

        IEnumerable<CardListingModel> All(string userId);

        IEnumerable<CardListingModel> GetCollection(string userId);

        bool AddCardToCollection(int cardId, string userId);

        bool RemoveCardFromCollection(int cardId, string userId);
    }
}
