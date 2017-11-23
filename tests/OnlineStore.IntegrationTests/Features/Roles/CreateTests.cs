namespace OnlineStore.IntegrationTests.Features.Roles
{
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using OnlineStore.Features.Roles;
    using Shouldly;

    public class CreateTests
    {
        public async Task CanCreate(SliceFixture fixture)
        {
            // Arrange
            var createCommand = new Create.Command
            {
                Name = "Role1"
            };

            // Act
            await fixture.SendAsync(createCommand);

            // Assert
            var roleInDb = await fixture.ExecuteDbContextAsync(db => db.Roles
                .FirstOrDefaultAsync(r => r.Name == createCommand.Name));

            roleInDb.Name.ShouldBe(createCommand.Name);
            roleInDb.NormalizedName.ShouldBe(createCommand.Name.ToUpper());
        }
    }
}
