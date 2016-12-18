namespace OnlineStore.Infrastructure.ViewModels.Products
{
    using System;

    public class ProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public DateTime DateAdded { get; set; }

        public string ImagePath { get; set; }
    }
}
