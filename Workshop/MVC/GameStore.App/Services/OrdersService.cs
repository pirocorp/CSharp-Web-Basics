namespace GameStore.App.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Data;
    using Data.Models;

    public class OrdersService : IOrdersService
    {
        private readonly GameStoreDbContext _db;

        public OrdersService(GameStoreDbContext db)
        {
            this._db = db;
        }

        public void Purchase(int userId, IEnumerable<int> gameIds)
        {
            var alreadyOwnedIds = this._db.Orders
                .Where(o => o.UserId == userId && gameIds.Contains(o.GameId))
                .Select(o => o.GameId)
                .ToList();

            var newGames = new List<int>(gameIds);

            foreach (var gameId in alreadyOwnedIds)
            {
                newGames.Remove(gameId);
            }

            foreach (var newGameId in newGames)
            {
                var order = new Order()
                {
                    GameId = newGameId,
                    UserId = userId
                };

                this._db.Orders.Add(order);
            }

            this._db.SaveChanges();
        }
    }
}
