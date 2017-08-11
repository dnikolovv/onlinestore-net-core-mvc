namespace OnlineStore.IntegrationTests.Features.Product
{
    using Microsoft.EntityFrameworkCore;
    using OnlineStore.Features.Product;
    using OnlineStore.Infrastructure.Constants;
    using OnlineStore.IntegrationTests.Extensions;
    using System.Threading.Tasks;

    public class ProductControllerTests
    {
        public async Task SuccessfullCreationSetsSuccessMessage(SliceFixture fixture)
        {
            // Arrange
            var controller = fixture.InstantiateController<ProductController>();

            // Act
            var createCommand = new Create.Command()
            {
                Name = "Some product",
                Description = "Some description",
                Category = new Data.Models.Category() { Name = "Some category" },
                Price = 120.00m
            };

            await controller.Create(createCommand);

            // Assert
            controller.TempData
                .ShouldContainSuccessMessage(SuccessMessages.SuccessfullyCreatedProduct(createCommand.Name));
        }

        public async Task SuccessfullEditSetsSuccessMessage(SliceFixture fixture)
        {
            // Arrange
            var product = await AddProductToDatabase(fixture);
            var controller = fixture.InstantiateController<ProductController>();

            // Act
            var editCommand = new Edit.Command
            {
                Id = product.Id,
                Name = "Edited name"
            };

            await controller.Edit(editCommand);

            controller.TempData
                .ShouldContainSuccessMessage(SuccessMessages.SuccessfullyEditedProduct(editCommand.Name));
        }

        public async Task SuccessfullRemoveSetsSuccessMessage(SliceFixture fixture)
        {
            // Arrange
            var product = AddProductToDatabase(fixture);
            var controller = fixture.InstantiateController<ProductController>();

            // Act
            var removeCommand = new Remove.Command
            {
                ProductId = product.Id
            };

            await controller.Remove(removeCommand);

            // Assert
            controller.TempData
                .ShouldContainSuccessMessage(SuccessMessages.SuccessfullyRemovedProduct(removeCommand.ProductId));
        }

        private static async Task<Data.Models.Product> AddProductToDatabase(SliceFixture fixture)
        {
            var createCommand = new Create.Command
            {
                Name = "Some product",
                Description = "Some description",
                Price = 31241.00m,
                Category = new Data.Models.Category() { Name = "Some category" },
            };

            await fixture.SendAsync(createCommand);

            var addedProduct = await fixture.ExecuteDbContextAsync(db => db
                .Products
                .FirstOrDefaultAsync(p => p.Name == createCommand.Name));
            return addedProduct;
        }
    }
}
