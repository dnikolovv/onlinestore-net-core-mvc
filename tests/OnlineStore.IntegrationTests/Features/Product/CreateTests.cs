namespace OnlineStore.IntegrationTests.Features.Product
{
    using Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using OnlineStore.Features.Product;
    using Shouldly;
    using System.Linq;
    using System.Threading.Tasks;

    public class CreateTests
    {
        public async Task QueryReturnsCorrectCommand(SliceFixture fixture)
        {
            var category1 = new Category() { Name = "Category1" };
            var category2 = new Category() { Name = "Category2" };

            await fixture.InsertAsync(category1, category2);

            var query = new Create.Query();

            var command = await fixture.SendAsync(query);

            var categoriesInDb = new SelectList(await fixture
                .ExecuteDbContextAsync(db => db
                    .Categories
                    .Select(c => c.Name)
                    .ToListAsync()));

            command.Categories.Count().ShouldBe(categoriesInDb.Count());
            command.Categories.First().Value.ShouldBe(categoriesInDb.First().Value);
            command.Categories.Skip(1).First().Value.ShouldBe(categoriesInDb.Skip(1).First().Value);
        }

        public async Task CanCreate(SliceFixture fixture)
        {
            var category = new Category
            {
                Name = "A category"
            };

            await fixture.InsertAsync(category);

            var command = new Create.Command
            {
                Name = "A product",
                Description = "A description",
                Category = category,
                Price = 120.00m
            };

            await fixture.SendAsync(command);

            var created = await fixture
                .ExecuteDbContextAsync(db => db
                    .Products
                    .Include(p => p.Category)
                    .Where(p => p.Name == command.Name)
                    .SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.Name.ShouldBe(command.Name);
            created.Price.ShouldBe(command.Price);
            created.Description.ShouldBe(command.Description);
            created.Category.Name.ShouldBe(category.Name);
        }
    }
}
