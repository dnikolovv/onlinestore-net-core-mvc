namespace OnlineStore.IntegrationTests.Features.Permissions
{
    using Microsoft.EntityFrameworkCore;
    using OnlineStore.Features.Permissions;
    using OnlineStore.Infrastructure.Constants;
    using OnlineStore.IntegrationTests.Extensions;
    using System.Threading.Tasks;

    public class PermissionControllerTests
    {
        public void SuccessfullCreationSetsSuccessMessage(SliceFixture fixture)
        {
            // Arrange
            var controller = fixture.GetController<PermissionController>();

            // Act
            var createCommand = new Create.Command
            {
                Action = "Some action",
                Controller = "Some controller"
            };

            controller.Create(createCommand);

            // Assert
            controller.TempData
                .ShouldContainSuccessMessage(SuccessMessages.SuccessfullyCreatedPermission());
        }

        public async Task SuccessfulEditSetsSuccessMessage(SliceFixture fixture)
        {
            // Arrange
            var controller = fixture.GetController<PermissionController>();
            var permission = await AddPermissionToDb(fixture);

            // Act
            var editCommand = new Edit.Command
            {
                Action = "Some edited action"
            };

            await controller.Edit(editCommand);

            // Assert
            controller.TempData
                .ShouldContainSuccessMessage(SuccessMessages.SuccessfullyEditedPermission());
        }

        public async Task SuccessfullRemovalSetsSuccessMessage(SliceFixture fixture)
        {
            // Arrange
            var controller = fixture.GetController<PermissionController>();
            var permission = await AddPermissionToDb(fixture);

            // Act
            var removeCommand = new Remove.Command
            {
                PermissionId = permission.Id
            };

            await controller.Remove(removeCommand);

            // Assert
            controller.TempData
                .ShouldContainSuccessMessage(SuccessMessages.SuccessfullyDeletedPermission((int)removeCommand.PermissionId));
        }

        private async Task<Data.Models.Permission> AddPermissionToDb(SliceFixture fixture)
        {
            var createCommand = new Create.Command
            {
                Action = "Some action",
                Controller = "Some controller"
            };

            await fixture.SendAsync(createCommand);

            return await fixture.ExecuteDbContextAsync(db => db
                .Permissions
                .FirstOrDefaultAsync(p => p.Action == createCommand.Action));
        }
    }
}
