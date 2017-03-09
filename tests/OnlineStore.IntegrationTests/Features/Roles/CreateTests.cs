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

    public class CreateTests
    {
        public async Task CanCreate(SliceFixture fixture)
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

            // Act
            await fixture.SendAsync(createCommand);

            // Assert
            var roleInDb = await fixture.ExecuteDbContextAsync(db => db.Roles
                .Include(r => r.PermissionsRoles)
                    .ThenInclude(pr => pr.Permission)
                .FirstOrDefaultAsync(r => r.Name == createCommand.Name));

            roleInDb.Name.ShouldBe(createCommand.Name);
            roleInDb.NormalizedName.ShouldBe(createCommand.Name.ToUpper());
            roleInDb.PermissionsRoles.Count.ShouldBe(1);
            roleInDb.PermissionsRoles.First().PermissionId.ShouldBe(permission.Id);
            roleInDb.PermissionsRoles.First().Permission.Action.ShouldBe(permission.Action);
            roleInDb.PermissionsRoles.First().Permission.Controller.ShouldBe(permission.Controller);
        }
    }
}
