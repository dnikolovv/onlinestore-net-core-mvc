namespace OnlineStore.IntegrationTests.Features.Category
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using System.Threading.Tasks;

    public class CreateTests
    {
        public async Task CanCreate(SliceFixture fixture)
        {
            // Arrange
            var category = new Category
            {
                Name = "Category"
            };

            // Act
            await fixture.InsertAsync(category);

            // Assert
            var categoryInDb = await fixture
                .ExecuteDbContextAsync(db => db
                .Categories
                .FirstOrDefaultAsync(c => c.Name == category.Name));

            categoryInDb.ShouldNotBeNull();
        }
    }
}
