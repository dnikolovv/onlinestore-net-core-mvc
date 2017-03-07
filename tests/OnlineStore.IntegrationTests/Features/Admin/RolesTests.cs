namespace OnlineStore.IntegrationTests.Features.Admin
{
    using Data.Models;
    using OnlineStore.Features.Admin;
    using Shouldly;
    using System.Linq;
    using System.Threading.Tasks;

    public class RolesTests
    {
        public async Task ListsAllRoles(SliceFixture fixture)
        {
            // Arrange
            UserRole[] roles = new UserRole[]
            {
                new UserRole
                {
                    Name = "Role1"
                },
                new UserRole
                {
                    Name = "Role2"
                },
                new UserRole
                {
                    Name = "Role3"
                }
            };

            await fixture.InsertAsync(roles);

            var query = new Roles.Query();

            // Act
            var response = await fixture.SendAsync(query);

            // Assert
            response.OrderBy(r => r.Name);

            response.Count().ShouldBe(roles.Length);
            response.ElementAt(0).Name.ShouldBe(roles[0].Name);
            response.ElementAt(1).Name.ShouldBe(roles[1].Name);
            response.ElementAt(2).Name.ShouldBe(roles[2].Name);
        }
    }
}
