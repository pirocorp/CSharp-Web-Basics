namespace GameStore.App.Infrastructure
{
    using Models.Orders;
    using WebServer.Http.Contracts;

    public static class HttpSessionExtensions
    {
        private const string SHOPPING_CART_SESSION_KEY = "%$Shopping_Cart$%";

        public static ShoppingCart GetShoppingCart(this IHttpSession session)
        {
            var shoppingCart = session.Get<ShoppingCart>(SHOPPING_CART_SESSION_KEY);

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart();
                session.Add(SHOPPING_CART_SESSION_KEY, shoppingCart);
            }

            return shoppingCart;
        }
    }
}
