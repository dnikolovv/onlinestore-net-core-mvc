namespace OnlineStore.IntegrationTests.Features.Order
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using OnlineStore.Features.Account;
    using OnlineStore.Features.Cart;
    using OnlineStore.Features.Order;
    using System;
    using System.Threading.Tasks;

    public class CompletedTests
    {
        public async Task ClearsUserCartOnCompletion(SliceFixture fixture)
        {
            // Arrange
            // Create a user
            var registerUserCommand = new Register.Command
            {
                FirstName = "John",
                UserName = "user",
                Password = "password123"
            };

            await fixture.SendAsync(registerUserCommand);

            // Create him some product to add
            var product = new Product
            {
                Name = "Product",
                Description = "Description",
                DateAdded = DateTime.Now,
                Category = new Category() { Name = "Category" },
                Price = 100.00m
            };

            await fixture.InsertAsync(product);

            var addItemQuery = new AddToCart.Command
            {
                CurrentUserName = registerUserCommand.UserName,
                ProductId = product.Id
            };

            await fixture.SendAsync(addItemQuery);

            // Act
            var query = new Completed.Command
            {
                CurrentUserName = registerUserCommand.UserName
            };

            await fixture.SendAsync(query);

            // Assert
            var userInDb = await fixture
                .ExecuteDbContextAsync(db => db
                .Users
                .Include(u => u.Cart)
                    .ThenInclude(c => c.OrderedItems)
                .FirstOrDefaultAsync(u => u.UserName == registerUserCommand.UserName));

            userInDb.Cart.OrderedItems.Count.ShouldBe(0);
        }
    }
}