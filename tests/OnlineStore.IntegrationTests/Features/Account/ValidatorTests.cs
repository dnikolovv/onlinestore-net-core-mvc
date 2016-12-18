namespace OnlineStore.IntegrationTests.Features.Account
{
    using OnlineStore.Features.Account;
    using OnlineStore.Features.Account.Util;
    using Microsoft.Extensions.DependencyInjection;
    using System.Threading.Tasks;
    using System.Threading;
    using Shouldly;

    public class ValidatorTests
    {
        public async Task FindsIfNameIsTaken(SliceFixture fixture)
        {
            // Arrange
            var registerUserCommand = new Register.Command
            {
                FirstName = "John",
                UserName = "user",
                Password = "password123"
            };

            await fixture.SendAsync(registerUserCommand);

            // Act
            var anotherRegisterCommand = new Register.Command
            {
                FirstName = "Mark",
                UserName = "user",
                Password = "secret123"
            };

            var nameNotTaken = true;

            await fixture.ExecuteScopeAsync(async sp =>
            {
                nameNotTaken = await sp.GetService<IUserValidator>()
                    .NameNotTakenAsync(anotherRegisterCommand.UserName, CancellationToken.None);
            });

            nameNotTaken.ShouldBeFalse();
        }

        public async Task PasswordMatchesConfirmation(SliceFixture fixture)
        {
            // Act
            var passwordsMatch = false;
            var password = "password123";

            await fixture.ExecuteScopeAsync(async sp =>
            { // That Task.Run() is actually unnessecary, but it is there to avoid the warning message
                await Task.Run(() =>
                {
                    passwordsMatch = sp.GetService<IUserValidator>()
                        .PasswordMatchesConfirmation(password, password);
                });
            });

            passwordsMatch.ShouldBeTrue();
        }

        public async Task PasswordDoesntMatchConfirmation(SliceFixture fixture)
        {
            // Act
            var passwordsMatch = true;
            var password = "password123";
            var anotherPassword = "asdgasdf";

            await fixture.ExecuteScopeAsync(async sp =>
            {
                await Task.Run(() =>
                { // That Task.Run() is actually unnessecary, but it is there to avoid the warning message
                    passwordsMatch = sp.GetService<IUserValidator>()
                        .PasswordMatchesConfirmation(password, anotherPassword);
                });
            });

            passwordsMatch.ShouldBeFalse();
        }
    }
}
