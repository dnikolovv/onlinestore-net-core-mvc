namespace OnlineStore.IntegrationTests.Features.Category
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using OnlineStore.Features.Category;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class RemoveTests
    {
        public async Task CanRemove(SliceFixture fixture)
        {
            // Arrange
            var category = new Category
            {
                Name = "Category"
            };

            await fixture.InsertAsync(category);

            // Act
            var removeCommand = new Remove.Command
            {
                Id = category.Id,
                Name = category.Name,
                ProductsInCategoryCount = 0
            };

            await fixture.SendAsync(removeCommand);

            // Assert
            var areThereCategoriesInTheDb = await fixture
                .ExecuteDbContextAsync(db => db
                .Categories
                .AnyAsync());

            areThereCategoriesInTheDb.ShouldBeFalse();
        }

        public async Task RemovingACategoryShouldRemoveTheProductsInIt(SliceFixture fixture)
        {
            // Arrange
            var category = new Category
            {
                Name = "Category"
            };

            await fixture.InsertAsync(category);

            var createProductsCommands = new List<OnlineStore.Features.Product.Create.Command>
            {
                new OnlineStore.Features.Product.Create.Command
                {
                    Name = "Product1",
                    Description = "Description1",
                    Category = category,
                    Price = 100.00m
                },
                new OnlineStore.Features.Product.Create.Command
                {
                    Name = "Product2",
                    Description = "Description2",
                    Category = category,
                    Price = 100.00m
                },
                new OnlineStore.Features.Product.Create.Command
                {
                    Name = "Product3",
                    Description = "Description3",
                    Category = category,
                    Price = 100.00m
                },
            };

            foreach (var command in createProductsCommands)
            {
                await fixture.SendAsync(command);
            }

            // Act
            var removeQuery = new Remove.Query
            {
                CategoryId = category.Id
            };

            var removeCommand = await fixture.SendAsync(removeQuery);

            await fixture.SendAsync(removeCommand);

            // Assert
            var areThereCategoriesInTheDb = await fixture
                .ExecuteDbContextAsync(db => db
                .Categories
                .AnyAsync());

            var areThereProductsInTheDb = await fixture
                .ExecuteDbContextAsync(db => db
                .Products
                .AnyAsync());

            areThereCategoriesInTheDb.ShouldBeFalse();
            areThereProductsInTheDb.ShouldBeFalse();
        }
    }
}
