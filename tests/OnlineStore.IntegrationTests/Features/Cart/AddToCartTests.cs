namespace OnlineStore.IntegrationTests.Features.Cart
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using OnlineStore.Features.Account;
    using OnlineStore.Features.Cart;
    using OnlineStore.Features.Product;
    using Shouldly;
    using System.Linq;
    using System.Threading.Tasks;

    public class AddToCartTests
    {
        public async Task CanAddToCart(SliceFixture fixture)
        {
            // Arrange
            var registeredUser = await RegisterUser(fixture);

            // Create him a product to add
            var createdProduct = await CreateSampleProduct(fixture);

            // Act
            var product = await fixture
                .ExecuteDbContextAsync(db => db
                .Products
                .FirstOrDefaultAsync(p => p.Name == createdProduct.Name));

            var addToCartCommand = new AddToCart.Command
            {
                CurrentUserName = registeredUser.UserName,
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
                .FirstOrDefaultAsync(u => u.UserName == registeredUser.UserName));

            user.Cart.OrderedItems.Count.ShouldBe(1);
            user.Cart.OrderedItems.First().Product.Name.ShouldBe(product.Name);
            user.Cart.OrderedItems.First().Quantity.ShouldBe(1);
        }

        private static async Task<Create.Command> CreateSampleProduct(SliceFixture fixture)
        {
            var createProductCommand = new Create.Command
            {
                Name = "Product",
                Description = "Description",
                Category = new Category { Name = "Category" },
                Price = 100.00m
            };

            await fixture.SendAsync(createProductCommand);
            return createProductCommand;
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
