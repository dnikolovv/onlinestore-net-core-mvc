namespace OnlineStore.IntegrationTests.Features.Roles
{
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using OnlineStore.Features.Roles;
    using OnlineStore.Infrastructure.Constants;
    using OnlineStore.IntegrationTests.Extensions;

    public class RoleControllerTests
    {
        public void SuccessfullCreationSetsSuccessMessage(SliceFixture fixture)
        {
            // Arrange
            var controller = fixture.GetController<RoleController>();

            // Act
            var createCommand = new Create.Command
            {
                Name = "Some role"
            };

            controller.Create(createCommand);

            // Assert
            controller.TempData
                .ShouldContainSuccessMessage(SuccessMessages.SuccessfullyCreatedRole(createCommand.Name));
        }

        public async Task SuccessfullEditSetsSuccessMessage(SliceFixture fixture)
        {
            // Arrange
            var controller = fixture.GetController<RoleController>();
            var role = AddRoleToDb(fixture);

            // Act
            var editCommand = new Edit.Command
            {
                Name = "Edited name"
            };

            await controller.Edit(editCommand);

            // Assert
            controller.TempData
                .ShouldContainSuccessMessage(SuccessMessages.SuccessfullyEditedRole(editCommand.Name));
        }

        public async Task SuccessfullRemovalSetsSuccessMessage(SliceFixture fixture)
        {
            // Arrange
            var controller = fixture.GetController<RoleController>();
            var role = AddRoleToDb(fixture);

            // Act
            var removeCommand = new Remove.Command
            {
                RoleId = role.Id
            };

            await controller.Remove(removeCommand);

            // Assert
            controller.TempData
                .ShouldContainSuccessMessage(SuccessMessages.SuccessfullyDeletedRole(role.Id));
        }

        private async Task<Data.Models.UserRole> AddRoleToDb(SliceFixture fixture)
        {
            var createCommand = new Create.Command
            {
                Name = "Some role"
            };

            await fixture.SendAsync(createCommand);

            return await fixture.ExecuteDbContextAsync(db => db
                .Roles
                .FirstOrDefaultAsync(r => r.Name == createCommand.Name));
        }
    }
}
