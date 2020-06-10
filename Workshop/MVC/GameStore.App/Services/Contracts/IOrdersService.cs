namespace GameStore.App.Services.Contracts
{
    using System.Collections.Generic;

    public interface IOrdersService
    {
        void Purchase(IEnumerable<int> gameIds);
    }
}
