namespace OnlineStore.IntegrationTests.Features.Roles
{
    using Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using OnlineStore.Features.Roles;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;

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

            var createCommand = new Create.Command
            {
                Name = "Role1",
                SelectedPermissions = new List<int> { permission.Id }
            };

            await fixture.SendAsync(createCommand);

            var roleInDb = await fixture.ExecuteDbContextAsync(db => db.Roles
                .Include(r => r.PermissionsRoles)
                    .ThenInclude(pr => pr.Permission)
                .FirstOrDefaultAsync(r => r.Name == createCommand.Name));

            roleInDb.PermissionsRoles.Count.ShouldBe(1);

            // Act
            var editCommand = new Edit.Command
            {
                Id = roleInDb.Id,
                Name = roleInDb.Name + "Edited",
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
    }
}
