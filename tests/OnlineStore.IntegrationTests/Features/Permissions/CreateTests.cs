namespace OnlineStore.IntegrationTests.Features.Permissions
{
    using Microsoft.EntityFrameworkCore;
    using OnlineStore.Features.Permissions;
    using Shouldly;
    using System.Threading.Tasks;

    public class CreateTests
    {
        public async Task CanCreate(SliceFixture fixture)
        {
            // Arrange
            var createCommand = new Create.Command
            {
                Action = "SomeAction",
                Controller = "SomeController"
            };

            // Act
            await fixture.SendAsync(createCommand);

            // Assert
            var permissionInDb = await fixture.ExecuteDbContextAsync(db => db.Permissions
                .FirstOrDefaultAsync(p => p.Action == createCommand.Action && p.Controller == createCommand.Controller));

            permissionInDb.ShouldNotBeNull();
        }
    }
}
