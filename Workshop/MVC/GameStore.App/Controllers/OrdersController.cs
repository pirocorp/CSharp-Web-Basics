namespace GameStore.App.Controllers
{
    using System.Linq;
    using Infrastructure;
    using Models.Orders;
    using Services.Contracts;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Contracts;

    public class OrdersController : BaseController
    {
        private readonly IGamesService _gamesService;

        public OrdersController(IGamesService gamesService)
        {
            this._gamesService = gamesService;
        }

        public IActionResult Buy(int id)
        {
            if (!this._gamesService.Exists(id))
            {
                return this.NotFound();
            }

            this.Request
                .Session
                .GetShoppingCart()
                .AddGame(id);

            return this.Redirect("/orders/cart");
        }

        public IActionResult Cart()
        {
            var shoppingCart = this.Request.Session.GetShoppingCart();

            var gameIds = shoppingCart.AllGames();

            var gamesToBuy = this._gamesService
                .ByIds<GameListingOrdersModel>(gameIds)
                .ToList();

            var allGames = gamesToBuy.Select(g => g.ToHtml());
            var totalPrice = gamesToBuy.Sum(g => g.Price);

            this.ViewModel["games"] = string.Join(string.Empty, allGames);
            this.ViewModel["total-price"] = totalPrice.ToString("F2");

            return this.View();
        }

        public IActionResult Remove(int id)
        {
            this.Request
                .Session
                .GetShoppingCart()
                .RemoveGame(id);

            return this.Redirect("/orders/cart");
        }

        [HttpPost]
        public IActionResult Finish(int id)
        {
            if (!this.User.IsAuthenticated)
            {
                return this.RedirectToLogin();
            }

            var shoppingCart = this.Request.Session.GetShoppingCart();

            var gameIds = shoppingCart.AllGames();

            return this.RedirectToHome();
        }
    }
}
