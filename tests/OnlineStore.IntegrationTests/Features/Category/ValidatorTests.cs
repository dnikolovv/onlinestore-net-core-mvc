namespace OnlineStore.IntegrationTests.Features.Category
{
    using Data.Models;
    using Microsoft.Extensions.DependencyInjection;
    using Shouldly;
    using OnlineStore.Features.Category.Util;
    using System.Threading;
    using System.Threading.Tasks;

    public class ValidatorTests
    {
        public async Task CannotCreateCategoryWithTheSameName(SliceFixture fixture)
        {
            // Arrange
            var category = new Category
            {
                Name = "Category"
            };

            await fixture.InsertAsync(category);

            // Act
            bool categoryDoesntExist = true;

            await fixture.ExecuteScopeAsync(async sp =>
            {
                categoryDoesntExist = await sp.GetService<ICategoryValidator>()
                    .CategoryDoesntExistAsync(category.Name, CancellationToken.None);
            });

            // Assert
            categoryDoesntExist.ShouldBeFalse();
        }
    }
}
