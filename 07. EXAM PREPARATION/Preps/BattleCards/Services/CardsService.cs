namespace BattleCards.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Data.Models;
    using Models.Cards;

    public class CardsService : ICardsService
    {
        private readonly BattleDbContext dbContext;

        public CardsService(BattleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CreateCard(
            string name,
            string imageUrl,
            string keyword,
            int attack,
            int health,
            string description)
        {
            var card = new Card()
            {
                Name = name,
                ImageUrl = imageUrl,
                Keyword = keyword,
                Attack = attack,
                Health = health,
                Description = description,
            };

            this.dbContext.Add(card);
            this.dbContext.SaveChanges();
        }

        public IEnumerable<CardListingModel> All(string userId)
            => this.dbContext.Cards
                .Select(c => new CardListingModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImageUrl = c.ImageUrl,
                    Keyword = c.Keyword,
                    Attack = c.Attack,
                    Health = c.Health,
                    Description = c.Description,
                    IsInCollection = c.Users.Any(u => u.UserId == userId)
                })
                .ToList();

        public IEnumerable<CardListingModel> GetCollection(string userId)
            => this.dbContext.Cards
                .Where(c => c.Users.Any(u => u.UserId == userId))
                .Select(c => new CardListingModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImageUrl = c.ImageUrl,
                    Keyword = c.Keyword,
                    Attack = c.Attack,
                    Health = c.Health,
                    Description = c.Description
                })
                .ToList();

        public bool AddCardToCollection(int cardId, string userId)
        {
            if (this.GetCardInCollection(cardId, userId) != null)
            {
                return false;
            }

            var userCard = new UserCard()
            {
                UserId = userId,
                CardId = cardId
            };

            this.dbContext.Add(userCard);
            this.dbContext.SaveChanges();

            return true;
        }

        public bool RemoveCardFromCollection(int cardId, string userId)
        {
            var userCard = this.GetCardInCollection(cardId, userId);

            if (userCard == null)
            {
                return false;
            }

            this.dbContext.UsersCards.Remove(userCard);
            this.dbContext.SaveChanges();

            return true;
        }

        private UserCard GetCardInCollection(int cardId, string userId)
            => this.dbContext.UsersCards.FirstOrDefault(c => c.CardId == cardId && c.UserId == userId);
    }
}