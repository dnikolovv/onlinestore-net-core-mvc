namespace OnlineStore.Data.Models
{
    public class CartItem : IEntity
    {
        public int Id { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }

        public Cart Cart { get; set; }
    }
}
