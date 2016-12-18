namespace OnlineStore.IntegrationTests.Features.Cart
{
    using Microsoft.EntityFrameworkCore;
    using OnlineStore.Features.Account;
    using OnlineStore.Features.Product;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Models;
    using System.Threading.Tasks;
    using OnlineStore.Features.Cart;
    using Shouldly;

    public class RemoveFromCartTests
    {
        public async Task CanRemoveFromCart(SliceFixture fixture)
        {
            // Arrange
            // Create a user who is going to remove his item
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

            // Send a remove command for the user and product
            var removeFromCartCommand = new RemoveFromCart.Command
            {
                CurrentUserName = registerUserCommand.UserName,
                ProductId = product.Id
            };

            await fixture.SendAsync(removeFromCartCommand);

            // Assert
            var user = await fixture
                .ExecuteDbContextAsync(db => db
                .Users
                .Include(u => u.Cart)
                    .ThenInclude(c => c.OrderedItems)
                .FirstOrDefaultAsync(u => u.UserName == registerUserCommand.UserName));

            user.Cart.OrderedItems.Count.ShouldBe(0);
        }
    }
}
