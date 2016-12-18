namespace OnlineStore.IntegrationTests.Features.Order
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using OnlineStore.Features.Account;
    using OnlineStore.Features.Cart;
    using OnlineStore.Features.Order;
    using OnlineStore.Features.Product;
    using System.Linq;
    using System.Threading.Tasks;

    public class CheckoutTests
    {
        public async Task CanCheckout(SliceFixture fixture)
        {
            // Arrange
            // Create a user
            var registerUserCommand = new Register.Command
            {
                FirstName = "John",
                UserName = "user",
                Password = "password123"
            };

            // Create him some product to buy
            var category = new Category
            {
                Name = "Category"
            };

            var createProductCommand = new Create.Command
            {
                Name = "Product",
                Price = 120.00m,
                Description = "Description",
                Category = category
            };

            // Send the commands so they persist
            await fixture.SendAsync(registerUserCommand);
            await fixture.InsertAsync(category);
            await fixture.SendAsync(createProductCommand);

            // Prepare the query to add the new product to the user's cart
            var productInDb = await fixture
                .ExecuteDbContextAsync(db => db
                .Products
                .FirstOrDefaultAsync(p => p.Name == createProductCommand.Name));

            var addToCartQuery = new AddToCart.Command() { CurrentUserName = registerUserCommand.UserName, ProductId = productInDb.Id };
            
            // Send it
            await fixture.SendAsync(addToCartQuery);

            // Act
            var userName = await fixture
                .ExecuteDbContextAsync(db => db
                .Users
                .Select(u => u.UserName)
                .FirstOrDefaultAsync(un => un == registerUserCommand.UserName));

            var query = new Checkout.Query
            {
                SenderUserName = userName
            };

            var command = await fixture.SendAsync(query);

            command.Name = "Some name";
            command.Line1 = "An address";
            command.City = "City";
            command.Country = "Country";
            command.Zip = "Zip";
            command.State = "State";

            await fixture.SendAsync(command);

            // Assert
            var orderInDb = await fixture
                .ExecuteDbContextAsync(db => db
                .Orders
                .FirstOrDefaultAsync(o => o.Name == command.Name));

            orderInDb.Name.ShouldBe(command.Name);
            orderInDb.Line1.ShouldBe(command.Line1);
            orderInDb.City.ShouldBe(command.City);
            orderInDb.Country.ShouldBe(command.Country);
            orderInDb.Zip.ShouldBe(command.Zip);
            orderInDb.State.ShouldBe(command.State);
        }
    }
}
