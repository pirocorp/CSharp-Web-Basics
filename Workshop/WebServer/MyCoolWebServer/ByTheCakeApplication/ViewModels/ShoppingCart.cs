namespace MyCoolWebServer.ByTheCakeApplication.ViewModels
{
    using System.Collections.Generic;

    public class ShoppingCart
    {
        public const string SessionKey = "%^Current_Shopping_Cart^%";

        public ShoppingCart()
        {
            this.ProductIds = new List<int>();
        }

        public List<int> ProductIds { get; }
    }
}
