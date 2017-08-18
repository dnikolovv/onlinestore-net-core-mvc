namespace OnlineStore.IntegrationTests.Features.Product
{
    using Data.Models;
    using OnlineStore.Features.Product;
    using Shouldly;
    using System.Linq;
    using System.Threading.Tasks;

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

            var createProductCommands = new Create.Command[]
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
            var productsReturnedArray = result.OrderByDescending(p => p.DateAdded).ToArray();

            productsReturnedArray.Count().ShouldBe(2);
            productsReturnedArray[0].Name.ShouldBe("Product5");
            productsReturnedArray[1].Name.ShouldBe("Product4");
        }
    }
}
