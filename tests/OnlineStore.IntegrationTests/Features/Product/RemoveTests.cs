namespace OnlineStore.IntegrationTests.Features.Product
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using OnlineStore.Features.Product;
    using System.Threading.Tasks;

    public class RemoveTests
    {
        public async Task CanRemoveProduct(SliceFixture fixture)
        {
            // Arrange
            var category = new Category
            {
                Name = "Category"
            };

            await fixture.InsertAsync(category);

            var createCommand = new Create.Command
            {
                Name = "Product",
                Description = "Description",
                Price = 12.00m,
                Category = category,
            };

            await fixture.SendAsync(createCommand);

            var product = await fixture
                .ExecuteDbContextAsync(db => db
                .Products
                .FirstOrDefaultAsync(p => p.Name == createCommand.Name));

            // Act
            var command = new Remove.Command() { ProductId = product.Id };

            await fixture.SendAsync(command);

            var productInDb = await fixture
                .ExecuteDbContextAsync(db => db
                .Products
                .FirstOrDefaultAsync(p => p.Id == product.Id));

            productInDb.ShouldBeNull();
        }
    }
}
