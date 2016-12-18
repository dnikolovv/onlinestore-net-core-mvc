namespace OnlineStore.Infrastructure.ViewModels.Cart
{
    using System.Collections.Generic;

    public class CartViewModel
    {
        public List<CartItemViewModel> OrderedItems { get; set; }

        public decimal TotalSum { get; set; }
    }
}
