namespace OnlineStore.IntegrationTests.Features.Roles
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using OnlineStore.Features.Roles;
    using Shouldly;

    public class EditTests
    {
        public async Task CanEdit(SliceFixture fixture)
        {
            // Arrange
            var role = new UserRole
            {
                Name = "Role1"
            };

            await fixture.InsertAsync(role);

            // Act
            var editCommand = new Edit.Command
            {
                Id = role.Id,
                Name = role.Name + "Edited",
                SelectedClaims = new List<string>()
            };

            await fixture.SendAsync(editCommand);

            // Assert
            var editedRole = await fixture.ExecuteDbContextAsync(db => db.Roles
                .FirstOrDefaultAsync(r => r.Id == editCommand.Id));

            editedRole.Name.ShouldBe(editCommand.Name);
            editedRole.NormalizedName.ShouldBe(editedRole.Name.ToUpper());
        }

        public async Task QueryReturnsCorrectResults(SliceFixture fixture)
        {
            // Arrange
            var role = new UserRole
            {
                Name = "Role1"
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
        }
    }
}
