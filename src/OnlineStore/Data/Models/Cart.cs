namespace OnlineStore.Data.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class Cart : IEntity
    {
        public Cart()
        {
            this.OrderedItems = new List<CartItem>();
        }

        public int Id { get; set; }

        public ICollection<CartItem> OrderedItems { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public decimal TotalSum()
        {
            return this.OrderedItems.Sum(i => i.Product.Price * i.Quantity);
        }
    }
}
