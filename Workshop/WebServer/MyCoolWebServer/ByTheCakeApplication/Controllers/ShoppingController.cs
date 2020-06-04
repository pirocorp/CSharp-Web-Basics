namespace MyCoolWebServer.ByTheCakeApplication.Controllers
{
    using System;
    using System.Linq;
    using Infrastructure;
    using Server.Http;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using Services;
    using ViewModels;

    public class ShoppingController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly IShoppingService _shoppingService;

        public ShoppingController()
        {
            this._userService = new UserService();
            this._productService = new ProductService();
            this._shoppingService = new ShoppingService();
        }

        public IHttpResponse AddToCart(IHttpRequest request)
        {
            var id = int.Parse(request.UrlParameters["id"]);

            if (!this._productService.Exists(id))
            {
                return new NotFoundResponse();
            }

            var shoppingCart = request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);
            shoppingCart.ProductIds.Add(id);

            var redirectUrl = "/search";

            const string searchTermKey = "searchTerm";

            if (request.UrlParameters.ContainsKey(searchTermKey))
            {
                redirectUrl = $"{redirectUrl}?{searchTermKey}={request.UrlParameters[searchTermKey]}";
            }

            return new RedirectResponse(redirectUrl);
        }

        public IHttpResponse ShowCart(IHttpRequest request)
        {
            var shoppingCart = request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            if (!shoppingCart.ProductIds.Any())
            {
                this.ViewData["cartItems"] = "No items in your cart";
                this.ViewData["totalCost"] = "0.00";
            }
            else
            {
                var productsInCart = this._productService
                    .FindProductsInCart(shoppingCart.ProductIds);

                var items = productsInCart
                    .Select(pr => $"<div>{pr.Name} - ${pr.Price:F2}</div><br />")
                    .ToArray();

                var totalPrice = productsInCart.Sum(pr => pr.Price);

                this.ViewData["cartItems"] = string.Join(Environment.NewLine, items);
                this.ViewData["totalCost"] = $"{totalPrice:F2}";
            }

            return this.FileViewResponse("Shopping/Cart");
        }

        public IHttpResponse FinishOrder(IHttpRequest request)
        { 
            var shoppingCart = request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);
            var username = request.Session.Get<string>(SessionStore.CurrentUserKey);

            var userId = this._userService.GetUserId(username);
            var productIds = shoppingCart.ProductIds;

            if (!userId.HasValue)
            {
                throw new InvalidOperationException($"User {username} does not exist");
            }

            if (!productIds.Any())
            {
                return new RedirectResponse("/");
            }

            this._shoppingService.CreateOrder(userId.Value, productIds);
            shoppingCart.ProductIds.Clear();

            return this.FileViewResponse("Shopping/Finish-Order");
        }
    }
}