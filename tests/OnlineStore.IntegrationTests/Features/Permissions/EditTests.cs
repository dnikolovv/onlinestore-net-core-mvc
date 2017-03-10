namespace OnlineStore.IntegrationTests.Features.Permissions
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using OnlineStore.Features.Permissions;
    using Shouldly;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class EditTests
    {
        public async Task CanEdit(SliceFixture fixture)
        {
            // Arrange
            var sampleRole = new UserRole
            {
                Name = "SomeRole"
            };

            await fixture.InsertAsync(sampleRole);

            var permission = new Permission
            {
                Action = "SomeAction",
                Controller = "SomeController",
                PermissionsRoles = new List<PermissionRole>
                {
                    new PermissionRole
                    {
                        RoleId = sampleRole.Id
                    }
                }
            };

            await fixture.InsertAsync(permission);

            var editCommand = new Edit.Command
            {
                Id = permission.Id,
                Action = permission.Action + "Edited",
                Controller = permission.Controller + "Edited",
                SelectedRoles = new List<int>()
            };

            // Act
            await fixture.SendAsync(editCommand);

            // Assert
            var permissionInDb = await fixture.ExecuteDbContextAsync(db => db.Permissions
                .Include(p => p.PermissionsRoles)
                .FirstOrDefaultAsync(p => p.Id == editCommand.Id));

            permissionInDb.Action.ShouldBe(editCommand.Action);
            permissionInDb.Controller.ShouldBe(editCommand.Controller);
            permissionInDb.PermissionsRoles.Count.ShouldBe(editCommand.SelectedRoles.Count);
        }

        public async Task QueryReturnsCorrectResults(SliceFixture fixture)
        {
            // Arrange
            var sampleRole = new UserRole
            {
                Name = "SomeRole"
            };

            await fixture.InsertAsync(sampleRole);

            var permission = new Permission
            {
                Action = "SomeAction",
                Controller = "SomeController",
                PermissionsRoles = new List<PermissionRole>
                {
                    new PermissionRole
                    {
                        RoleId = sampleRole.Id
                    }
                }
            };

            await fixture.InsertAsync(permission);

            var query = new Edit.Query
            {
                PermissionId = permission.Id
            };

            // Act
            var response = await fixture.SendAsync(query);

            // Assert
            response.Action.ShouldBe(permission.Action);
            response.Controller.ShouldBe(permission.Controller);
            response.SelectedRoles.Count.ShouldBe(permission.PermissionsRoles.Count);
            response.AvailableRoles.Count.ShouldBe(1);
        }
    }
}
