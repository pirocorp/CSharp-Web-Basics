﻿namespace MyCoolWebServer.ByTheCakeApplication.Services
{
    using System.Collections.Generic;
    using ViewModels.Products;

    public interface IProductService
    {
        void Create(string name, decimal price, string imageUrl);

        IEnumerable<ProductListingViewModel> All(string searchTerm = null);

        ProductDetailsViewModels Find(int id);

        bool Exists(int id);

        IEnumerable<ProductInCartViewModel> FindProductsInCart(IEnumerable<int> ids);
    }
}
