namespace OnlineStore.IntegrationTests.Features.Cart
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using OnlineStore.Features.Account;
    using OnlineStore.Features.Cart;
    using OnlineStore.Features.Product;
    using System.Linq;
    using System.Threading.Tasks;

    public class AddToCartTests
    {
        public async Task CanAddToCart(SliceFixture fixture)
        {
            // Arrange
            // Create a user who is going to add his item
            var registerUserCommand = new Register.Command
            {
                FirstName = "John",
                UserName = "user",
                Password = "password123"
            };

            await fixture.SendAsync(registerUserCommand);

            // Create him a product to add
            var createProductCommand = new Create.Command
            {
                Name = "Product",
                Description = "Description",
                Category = new Category { Name = "Category" },
                Price = 100.00m
            };

            await fixture.SendAsync(createProductCommand);

            // Act
            // Get the product that was created by the command
            var product = await fixture
                .ExecuteDbContextAsync(db => db
                .Products
                .FirstOrDefaultAsync(p => p.Name == createProductCommand.Name));

            var addToCartCommand = new AddToCart.Command
            {
                CurrentUserName = registerUserCommand.UserName,
                ProductId = product.Id
            };

            await fixture.SendAsync(addToCartCommand);

            // Assert
            var user = await fixture
                .ExecuteDbContextAsync(db => db
                .Users
                .Include(u => u.Cart)
                    .ThenInclude(c => c.OrderedItems)
                        .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(u => u.UserName == registerUserCommand.UserName));

            user.Cart.OrderedItems.Count.ShouldBe(1);
            user.Cart.OrderedItems.First().Product.Name.ShouldBe(product.Name);
            user.Cart.OrderedItems.First().Quantity.ShouldBe(1);
        }
    }
}
