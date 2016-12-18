namespace OnlineStore.Infrastructure.ViewModels.Orders
{
    using Data.Models;
    using System.Collections.Generic;

    public class OrderViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Zip { get; set; }

        public ICollection<CartItem> OrderedItems { get; set; }
    }
}
