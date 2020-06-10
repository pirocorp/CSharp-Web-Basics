namespace GameStore.App.Models.Orders
{
    using System.Collections.Generic;

    public class ShoppingCart
    {
        private readonly ICollection<int> _gameIds;

        public ShoppingCart()
        {
            this._gameIds = new List<int>();
        }

        public void AddGame(int gameId)
        {
            if (!this._gameIds.Contains(gameId))
            {
                this._gameIds.Add(gameId);
            }

        }

        public IEnumerable<int> AllGames() => new List<int>(this._gameIds);

        public void RemoveGame(int gameId) => this._gameIds.Remove(gameId);

        public void Clear() => this._gameIds.Clear();
    }
}
