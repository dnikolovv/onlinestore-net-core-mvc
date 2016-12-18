namespace OnlineStore.IntegrationTests.Features.Category
{
    using AutoMapper;
    using Data.Models;
    using Infrastructure.ViewModels.Categories;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using OnlineStore.Features.Category;
    using System.Threading.Tasks;

    public class EditTests
    {
        public async Task QueryReturnsCorrectResult(SliceFixture fixture)
        {
            // Arrange
            var category = new Category
            {
                Name = "Category"
            };

            await fixture.InsertAsync(category);

            // Act
            var editQuery = new Edit.Query
            {
                CategoryId = category.Id
            };

            var command = await fixture.SendAsync(editQuery);

            // Assert
            command.Id.ShouldBe(category.Id);
            command.Name.ShouldBe(category.Name);
        }

        public async Task CanEdit(SliceFixture fixture)
        {
            // Arrange
            var category = new Category
            {
                Name = "Category"
            };

            await fixture.InsertAsync(category);

            // Act
            var editQuery = new Edit.Query
            {
                CategoryId = category.Id
            };

            CategoryEditViewModel viewModel = await fixture.SendAsync(editQuery);

            var command = Mapper.Map<Edit.Command>(viewModel);

            command.Name = "New category name";

            await fixture.SendAsync(command);

            // Assert
            var categoryInDb = await fixture
                .ExecuteDbContextAsync(db => db
                .Categories
                .FirstOrDefaultAsync(c => c.Id == category.Id));

            categoryInDb.Name.ShouldBe(command.Name);
        }
    }
}
