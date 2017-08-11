namespace OnlineStore.IntegrationTests.Features.Category
{
    using Microsoft.EntityFrameworkCore;
    using OnlineStore.Features.Category;
    using OnlineStore.Infrastructure.Constants;
    using OnlineStore.IntegrationTests.Extensions;
    using System.Threading.Tasks;

    public class CategoryControllerTests
    {
        public async Task SuccessfulCreationSetsSuccessMessage(SliceFixture fixture)
        {
            // Arrange
            var controller = fixture.InstantiateController<CategoryController>();

            // Act
            var createCommand = new Create.Command { Name = "Some category" };
            await controller.Create(createCommand);

            // Assert
            controller.TempData
                .ShouldContainSuccessMessage(SuccessMessages.SuccessfullyCreatedCategory(createCommand.Name));
        }

        public async Task SuccessfullEditSetsSuccessMessage(SliceFixture fixture)
        {
            // Arrange
            var category = AddCategoryToDatabase(fixture);
            var controller = fixture.InstantiateController<CategoryController>();

            // Act
            var editCommand = new Edit.Command { Id = category.Id, Name = "Edited name" };
            await controller.Edit(editCommand);

            // Assert
            controller.TempData
                .ShouldContainSuccessMessage(SuccessMessages.SuccessfullyEditedCategory(editCommand.Name));
        }

        public async Task SuccessfullRemoveSetsSuccessMessage(SliceFixture fixture)
        {
            // Arrange
            var category = AddCategoryToDatabase(fixture);
            var controller = fixture.InstantiateController<CategoryController>();

            // Act
            var removeCommand = new Remove.Command { Id = category.Id };
            await controller.Remove(removeCommand);

            // Assert
            controller.TempData
                .ShouldContainSuccessMessage(SuccessMessages.SuccessfullyRemovedCategory(removeCommand.Name));
        }

        private async Task<Data.Models.Category> AddCategoryToDatabase(SliceFixture fixture)
        {
            var createCommand = new Create.Command
            {
                Name = "Some category"
            };

            await fixture.SendAsync(createCommand);

            return await fixture.ExecuteDbContextAsync(db => db
                .Categories
                .FirstOrDefaultAsync(c => c.Name == createCommand.Name));
        }
    }
}
