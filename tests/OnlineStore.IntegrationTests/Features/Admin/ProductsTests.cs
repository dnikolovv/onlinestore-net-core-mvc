namespace OnlineStore.IntegrationTests.Features.Admin
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using OnlineStore.Features.Admin;
    using OnlineStore.Features.Product;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductsTests
    {
        public async Task ListsAllProductsByDate(SliceFixture fixture)
        {
            // Arrange
            var category = new Category
            {
                Name = "Category"
            };

            await fixture.InsertAsync(category);

            List<Create.Command> createProductsCommands = new List<Create.Command>
            {
                new Create.Command
                {
                    Name = "Product",
                    Description = "Description",
                    Category = category,
                    Price = 100.00m
                },
                new Create.Command
                {
                    Name = "Product1",
                    Description = "Description",
                    Category = category,
                    Price = 100.00m
                },
                new Create.Command
                {
                    Name = "Product2",
                    Description = "Description",
                    Category = category,
                    Price = 100.00m
                },
            };

            foreach (var command in createProductsCommands)
            {
                await fixture.SendAsync(command);
            }

            // Act
            var query = new Products.Query();

            var result = await fixture.SendAsync(query);

            // Assert
            var productsInDb = await fixture
                .ExecuteDbContextAsync(db => db
                .Products
                .OrderByDescending(p => p.DateAdded)
                .ToListAsync());

            result.Count().ShouldBe(createProductsCommands.Count);
            result.Select(p => p.DateAdded)
                .SequenceEqual(productsInDb.Select(p => p.DateAdded))
                .ShouldBeTrue();
        }
    }
}
