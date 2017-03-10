namespace OnlineStore.IntegrationTests.Features.Roles
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using OnlineStore.Features.Roles;
    using Shouldly;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class EditTests
    {
        public async Task CanEdit(SliceFixture fixture)
        {
            // Arrange
            var permission = new Permission
            {
                Action = "Action1",
                Controller = "Controller1"
            };

            await fixture.InsertAsync(permission);

            var role = new UserRole
            {
                Name = "Role1",
                PermissionsRoles = new List<PermissionRole>
                {
                    new PermissionRole
                    {
                        PermissionId = permission.Id
                    }
                }
            };

            await fixture.InsertAsync(role);

            // Act
            var editCommand = new Edit.Command
            {
                Id = role.Id,
                Name = role.Name + "Edited",
                SelectedPermissions = new List<int>()
            };

            await fixture.SendAsync(editCommand);

            // Assert
            var editedRole = await fixture.ExecuteDbContextAsync(db => db.Roles
                .Include(r => r.PermissionsRoles)
                .FirstOrDefaultAsync(r => r.Id == editCommand.Id));

            editedRole.Name.ShouldBe(editCommand.Name);
            editedRole.NormalizedName.ShouldBe(editedRole.Name.ToUpper());
            editedRole.PermissionsRoles.Count.ShouldBe(editCommand.SelectedPermissions.Count);
        }

        public async Task QueryReturnsCorrectResults(SliceFixture fixture)
        {
            // Arrange
            var permission = new Permission
            {
                Action = "Action1",
                Controller = "Controller1"
            };

            await fixture.InsertAsync(permission);

            var role = new UserRole
            {
                Name = "Role1",
                PermissionsRoles = new List<PermissionRole>
                {
                    new PermissionRole
                    {
                        PermissionId = permission.Id
                    }
                }
            };

            await fixture.InsertAsync(role);

            var query = new Edit.Query
            {
                RoleId = role.Id
            };

            // Act
            var model = await fixture.SendAsync(query);

            // Assert
            model.Id.ShouldBe(role.Id);
            model.Name.ShouldBe(role.Name);
            model.SelectedPermissions.Count.ShouldBe(role.PermissionsRoles.Count);
            model.SelectedPermissions.First().ShouldBe(permission.Id);
            model.AvailablePermissions.Count.ShouldBe(1);
            model.AvailablePermissions.First().Id.ShouldBe(permission.Id);
        }
    }
}
