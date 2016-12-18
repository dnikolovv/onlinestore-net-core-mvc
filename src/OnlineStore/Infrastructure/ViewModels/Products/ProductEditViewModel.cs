namespace OnlineStore.Infrastructure.ViewModels.Products
{
    using Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class ProductEditViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public decimal Price { get; set; }

        public Category Category { get; set; }

        public SelectList Categories { get; set; }
    }
}
