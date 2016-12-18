namespace OnlineStore.Infrastructure.ViewModels.Cart
{
    using System.Collections.Generic;

    public class CartIndexViewModel
    {
        public List<CartItemViewModel> OrderedItems { get; set; }

        public decimal TotalSum { get; set; }

        public string ReturnUrl { get; set; }
    }
}
