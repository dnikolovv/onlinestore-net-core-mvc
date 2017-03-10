namespace OnlineStore.IntegrationTests.Features.Permissions
{
    using OnlineStore.Features.Permissions;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using OnlineStore.Features.Permissions.Util;
    using System.Threading;
    using Shouldly;

    public class ValidatorTests
    {
        public async Task DetectsIfPermissionIsDefined(SliceFixture fixture)
        {
            // Arrange
            var createCommand = new Create.Command
            {
                Action = "SomeAction",
                Controller = "SomeController"
            };

            await fixture.SendAsync(createCommand);

            // Act
            bool permissionIsNotDefined = await fixture.ExecuteScopeAsync(sp =>
                sp.GetService<IPermissionValidator>().PermissionDoesntExistAsync(createCommand.Controller, createCommand.Action, CancellationToken.None));

            // Assert
            permissionIsNotDefined.ShouldBeFalse();
        }
    }
}
