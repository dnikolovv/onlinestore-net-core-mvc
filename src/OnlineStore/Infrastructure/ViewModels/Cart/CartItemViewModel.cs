namespace OnlineStore.Infrastructure.ViewModels.Cart
{
    using Products;

    public class CartItemViewModel
    {
        public ProductViewModel Product { get; set; }

        public int Quantity { get; set; }
    }
}
