namespace OnlineStore.IntegrationTests.Features.Product
{
    using AutoMapper;
    using Data.Models;
    using Infrastructure.ViewModels.Products;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using OnlineStore.Features.Product;
    using System.Linq;
    using System.Threading.Tasks;

    public class EditTests
    {
        public async Task QueryReturnsCorrectCommand(SliceFixture fixture)
        {
            // Arrange
            var category = new Category() { Name = "Category1" };
            var secondCategory = new Category() { Name = "Category2" };

            await fixture.InsertAsync(category, secondCategory);

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
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Name == createCommand.Name));

            // Act
            var query = new Edit.Query() { ProductId = product.Id };
            var command = await fixture.SendAsync(query);

            // Assert
            var categoriesInDb = new SelectList(await fixture
                .ExecuteDbContextAsync(db => db
                    .Categories
                    .Select(c => c.Name)
                    .ToListAsync()));

            command.Name.ShouldBe(product.Name);
            command.Description.ShouldBe(product.Description);
            command.Price.ShouldBe(product.Price);
            command.Category.Name.ShouldBe(product.Category.Name);
            command.Categories.Count().ShouldBe(categoriesInDb.Count());
            command.Categories.First().Value.ShouldBe(categoriesInDb.First().Value);
            command.Categories.Skip(1).First().Value.ShouldBe(categoriesInDb.Skip(1).First().Value);
        }

        public async Task CanEditProduct(SliceFixture fixture)
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
            // Send the query with the product's id
            var query = new Edit.Query() { ProductId = product.Id };

            ProductEditViewModel viewModel = await fixture.SendAsync(query);

            var command = Mapper.Map<Edit.Command>(viewModel);

            // Change up some values
            command.Name = "New product name";
            command.Description = "New description";
            command.Price = 100.00m;

            // Send the command
            await fixture.SendAsync(command);

            // Assert
            // Get the now updated product
            var modifiedProduct = await fixture
                .ExecuteDbContextAsync(db => db
                .Products
                .FirstOrDefaultAsync(p => p.Id == product.Id));

            modifiedProduct.Name.ShouldBe(command.Name);
            modifiedProduct.Description.ShouldBe(command.Description);
            modifiedProduct.Price.ShouldBe(command.Price);
        }
    }
}
