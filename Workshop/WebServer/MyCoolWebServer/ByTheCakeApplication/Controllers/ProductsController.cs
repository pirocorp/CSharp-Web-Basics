namespace MyCoolWebServer.ByTheCakeApplication.Controllers
{
    using System;
    using System.Linq;
    using Infrastructure;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using Services;
    using ViewModels;
    using ViewModels.Products;

    public class ProductsController : BaseController
    {
        private const string ADD_VIEW = "Products/Add";

        private readonly IProductService _productService;

        public ProductsController()
        {
            this._productService = new ProductService();
        }

        public IHttpResponse Add()
        {
            this.ViewData["showResult"] = "none";

            return this.FileViewResponse(ADD_VIEW);
        }

        public IHttpResponse Add(AddProductViewModel model)
        {
            if (model.Name.Length < 3
                || model.Name.Length > 30
                || model.ImageUrl.Length < 3
                || model.ImageUrl.Length > 2000)
            {
                this.ShowError("Invalid product information.");

                return this.FileViewResponse(ADD_VIEW);
            }

            this._productService.Create(model.Name, model.Price, model.ImageUrl);

            this.ViewData["name"] = model.Name;
            this.ViewData["price"] = model.Price.ToString("F2");
            this.ViewData["imageUrl"] = model.ImageUrl;
            this.ViewData["showResult"] = "block";

            return this.FileViewResponse(ADD_VIEW);
        }

        public IHttpResponse Search(IHttpRequest request)
        {
            const string searchTermKey = "searchTerm";
            
            var urlParameters = request.UrlParameters;
            
            var searchTerm = urlParameters.ContainsKey(searchTermKey) 
                ? urlParameters[searchTermKey]
                : default;

            var results = "No cakes found!";
            this.ViewData["searchTerm"] = searchTerm;

            var result = this._productService.All(searchTerm);

            if (!result.Any())
            {
                this.ViewData["results"] = results;
            }
            else
            {
                var allProducts = result
                    .Select(c => $@"<div><a href=""/cakes/{c.Id}"">{c.Name}</a> - ${c.Price:F2} <a href=""/shopping/add/{c.Id}?searchTerm={searchTerm}"">Order</a></div>")
                    .ToArray();

                this.ViewData["results"] = string.Join(Environment.NewLine, allProducts);
            }

            //Shopping Cart logic
            this.ViewData["showCart"] = "none";
            var shoppingCart = request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            if (shoppingCart.ProductIds.Any())
            {
                var totalProducts = shoppingCart.ProductIds.Count;
                var totalProductsText = totalProducts != 1 ? "products" : "product";

                this.ViewData["showCart"] = "block";
                this.ViewData["products"] = $"{totalProducts} {totalProductsText}";
            }

            return this.FileViewResponse("Products/Search");
        }

        public IHttpResponse Details(int id)
        {
            var product = this._productService.Find(id);

            if (product == null)
            {
                return new NotFoundResponse();
            }

            this.ViewData["name"] = product.Name;
            this.ViewData["price"] = product.Price.ToString("F2");
            this.ViewData["imageUrl"] = product.ImageUrl;

            return this.FileViewResponse("Products/Details");
        }
    }
}
