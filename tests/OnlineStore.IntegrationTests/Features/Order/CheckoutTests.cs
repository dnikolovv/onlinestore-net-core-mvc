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
            var registeredUser = await RegisterUser(fixture);
            Product productInDb = await CreateSampleProduct(fixture);
            await AddProductToCart(fixture, registeredUser, productInDb);

            // Act
            var command = await Checkout(fixture, registeredUser);

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

        private static async Task<Checkout.Command> Checkout(SliceFixture fixture, Register.Command registerUserCommand)
        {
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
            return command;
        }

        private static async Task AddProductToCart(SliceFixture fixture, Register.Command registerUserCommand, Product productInDb)
        {
            var addToCartQuery = new AddToCart.Command() { CurrentUserName = registerUserCommand.UserName, ProductId = productInDb.Id };

            await fixture.SendAsync(addToCartQuery);
        }

        private static async Task<Product> CreateSampleProduct(SliceFixture fixture)
        {
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

            await fixture.InsertAsync(category);
            await fixture.SendAsync(createProductCommand);

            var productInDb = await fixture
                .ExecuteDbContextAsync(db => db
                .Products
                .FirstOrDefaultAsync(p => p.Name == createProductCommand.Name));
            return productInDb;
        }

        private static async Task<Register.Command> RegisterUser(SliceFixture fixture)
        {
            var registerUserCommand = new Register.Command
            {
                FirstName = "John",
                UserName = "user",
                Password = "password123"
            };

            await fixture.SendAsync(registerUserCommand);
            return registerUserCommand;
        }
    }
}
