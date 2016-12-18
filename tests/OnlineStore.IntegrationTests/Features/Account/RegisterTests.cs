namespace OnlineStore.IntegrationTests.Features.Account
{
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using OnlineStore.Features.Account;
    using System.Threading.Tasks;

    public class RegisterTests
    {
        public async Task CanRegister(SliceFixture fixture)
        {
            // Arrange
            var registerUserCommand = new Register.Command
            {
                FirstName = "John",
                UserName = "user",
                Password = "password123"
            };

            // Act
            await fixture.SendAsync(registerUserCommand);

            // Assert
            var user = await fixture
                .ExecuteDbContextAsync(db => db
                .Users
                .FirstOrDefaultAsync(u => u.UserName == registerUserCommand.UserName));

            user.ShouldNotBeNull();
            user.FirstName.ShouldBe(registerUserCommand.FirstName);
        }

        public async Task UserHasACartAfterRegistration(SliceFixture fixture)
        {
            // Arrange
            var registerUserCommand = new Register.Command
            {
                FirstName = "John",
                UserName = "user",
                Password = "password123"
            };

            // Act
            await fixture.SendAsync(registerUserCommand);

            // Assert
            var user = await fixture
                .ExecuteDbContextAsync(db => db
                .Users
                    .Include(u => u.Cart)
                        .ThenInclude(c => c.OrderedItems)
                .FirstOrDefaultAsync(u => u.UserName == registerUserCommand.UserName));

            user.Cart.ShouldNotBeNull();
            user.Cart.OrderedItems.ShouldNotBeNull();
        }
    }
}
