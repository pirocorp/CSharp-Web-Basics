namespace GameStore.App.Models.Games
{
    using Data.Models;
    using Infrastructure.Mapping;

    public class GameListingAdminModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Size { get; set; }

        public decimal Price { get; set; }
    }
}
