namespace GameStore.App.Services.Contracts
{
    using System.Collections.Generic;

    public interface IOrdersService
    {
        void Purchase(int userId, IEnumerable<int> gameIds);
    }
}
