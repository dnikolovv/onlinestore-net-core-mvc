namespace OnlineStore.IntegrationTests.Features.Admin
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using OnlineStore.Features.Admin;
    using Shouldly;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PermissionsTests
    {
        public async Task ListsAllPermissions(SliceFixture fixture)
        {
            // Arrange
            var sampleRole = new UserRole
            {
                Name = "Role1"
            };

            await fixture.InsertAsync(sampleRole);

            var permissions = new Permission[]
            {
                new Permission
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
                },
                new Permission
                {
                    Action = "SomeAction1",
                    Controller = "SomeController1",
                    PermissionsRoles = new List<PermissionRole>()
                }
            };

            await fixture.InsertAsync(permissions);

            var query = new Permissions.Query();

            // Act
            var response = await fixture.SendAsync(query);

            // Assert
            var permissionsInDb = await fixture.ExecuteDbContextAsync(db => db.Permissions
                .Include(p => p.PermissionsRoles)
                .ToListAsync());

            // So I can compare by index
            response.OrderBy(r => r.Id);
            permissionsInDb.OrderBy(r => r.Id);

            response.ElementAt(0).Id.ShouldBe(permissionsInDb[0].Id);
            response.ElementAt(1).Id.ShouldBe(permissionsInDb[1].Id);
            response.ElementAt(0).Action.ShouldBe(permissionsInDb[0].Action);
            response.ElementAt(1).Action.ShouldBe(permissionsInDb[1].Action);
            response.ElementAt(0).Controller.ShouldBe(permissionsInDb[0].Controller);
            response.ElementAt(1).Controller.ShouldBe(permissionsInDb[1].Controller);
            response.ElementAt(0).PermissionsRolesCount.ShouldBe(permissionsInDb[0].PermissionsRoles.Count);
            response.ElementAt(1).PermissionsRolesCount.ShouldBe(permissionsInDb[1].PermissionsRoles.Count);
        }
    }
}
