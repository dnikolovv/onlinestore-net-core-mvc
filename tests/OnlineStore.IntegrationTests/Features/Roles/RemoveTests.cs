namespace OnlineStore.IntegrationTests.Features.Roles
{
    using Microsoft.EntityFrameworkCore;
    using OnlineStore.Features.Roles;
    using Shouldly;
    using System.Threading.Tasks;

    public class RemoveTests
    {
        public async Task CanRemove(SliceFixture fixture)
        {
            // Arrange
            var createCommand = new Create.Command { Name = "Some role" };
            await fixture.SendAsync(createCommand);

            var addedRole = await fixture.ExecuteDbContextAsync(db => db
                .Roles
                .FirstOrDefaultAsync(r => r.Name == createCommand.Name));

            var removeCommand = new Remove.Command
            {
                RoleId = addedRole.Id
            };

            // Act
            await fixture.SendAsync(removeCommand);

            // Assert
            var roleAfterDeletion = await fixture.ExecuteDbContextAsync(db => db
                .Roles
                .FirstOrDefaultAsync(r => r.Id == removeCommand.RoleId));

            roleAfterDeletion.ShouldBeNull();
        }
    }
}
