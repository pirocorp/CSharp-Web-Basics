namespace BattleCards.Models.Cards
{
    public class CardListingModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string ImageUrl { get; init; }

        public string Keyword { get; init; }

        public int Attack { get; init; }

        public int Health { get; init; }

        public string Description { get; init; }

        public bool IsInCollection { get; init; }
    }
}
