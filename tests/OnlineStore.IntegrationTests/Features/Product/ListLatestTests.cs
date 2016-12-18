namespace OnlineStore.IntegrationTests.Features.Product
{
    using OnlineStore.Features.Product;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data.Models;
    using System.Linq;
    using Shouldly;

    public class ListLatestTests
    {
        public async Task ReturnsLatestProducts(SliceFixture fixture)
        {
            // Arrange
            var category = new Category
            {
                Name = "Category"
            };

            await fixture.InsertAsync(category);

            var createProductCommands = new List<Create.Command>
            {
                new Create.Command
                {
                    Name = "Product1",
                    Description = "Description1",
                    Price = 100.00m,
                    Category = category
                },
                new Create.Command
                {
                    Name = "Product2",
                    Description = "Description1",
                    Price = 100.00m,
                    Category = category
                },
                new Create.Command
                {
                    Name = "Product3",
                    Description = "Description1",
                    Price = 100.00m,
                    Category = category
                },
                new Create.Command
                {
                    Name = "Product4",
                    Description = "Description1",
                    Price = 100.00m,
                    Category = category
                },
                new Create.Command
                {
                    Name = "Product5",
                    Description = "Description1",
                    Price = 100.00m,
                    Category = category
                }
            };

            foreach (var command in createProductCommands)
            {
                await fixture.SendAsync(command);
            }

            // Act
            var query = new ListLatest.Query
            {
                NumberOfItems = 2
            };

            var result = await fixture.SendAsync(query);

            // Assert
            var productsReturnedArray = result.ToArray();

            productsReturnedArray.Count().ShouldBe(2);
            productsReturnedArray[0].Name.ShouldBe("Product5");
            productsReturnedArray[1].Name.ShouldBe("Product4");
        }
    }
}
