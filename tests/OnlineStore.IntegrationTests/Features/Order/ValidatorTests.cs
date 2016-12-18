namespace OnlineStore.IntegrationTests.Features.Order
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Shouldly;
    using OnlineStore.Features.Account;
    using OnlineStore.Features.Order.Util;
    using System.Threading;
    using System.Threading.Tasks;

    public class ValidatorTests
    {
        public async Task CannotCheckoutWithEmptyCart(SliceFixture fixture)
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

            // Act
            var user = await fixture
                .ExecuteDbContextAsync(db => db
                .Users
                .FirstOrDefaultAsync(u => u.UserName == registerUserCommand.UserName));

            bool canCheckout = true;

            // Get the custom validator and check whether it allows for checkout
            await fixture.ExecuteScopeAsync(async sp =>
            {
                canCheckout = await sp.GetService<IOrderValidator>()
                    .UserHasItemsInCartAsync(user.UserName, CancellationToken.None);
            });

            // Assert
            canCheckout.ShouldBeFalse();
        }
    }
}
