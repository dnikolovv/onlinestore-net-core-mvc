namespace OnlineStore.IntegrationTests.Features.Admin
{
    using Microsoft.EntityFrameworkCore;
    using OnlineStore.Features.Account;
    using OnlineStore.Features.Admin;
    using Shouldly;
    using System.Linq;
    using System.Threading.Tasks;

    public class UsersTests
    {
        public async Task ListsAllUsers(SliceFixture fixture)
        {
            // Arrange
            var registerUserCommand = new Register.Command
            {
                UserName = "User1",
                FirstName = "Some name",
                Password = "password",
                ConfirmedPassword = "password"
            };

            var registerUserCommand2 = new Register.Command
            {
                UserName = "User2",
                FirstName = "Some name",
                Password = "password",
                ConfirmedPassword = "password"
            };

            await fixture.SendAsync(registerUserCommand);
            await fixture.SendAsync(registerUserCommand2);

            // Act
            var query = new Users.Query();
            var model = await fixture.SendAsync(query);

            // Assert
            var usersInDb = await fixture.ExecuteDbContextAsync(db =>
                db.Users
                .Include(u => u.Roles)
                .ToListAsync());

            // Order them so I can compare by index
            model = model.OrderBy(u => u.Id);
            usersInDb = usersInDb.OrderBy(u => u.Id).ToList();

            usersInDb.Count.ShouldBe(model.Count());
            model.ElementAt(0).Id.ShouldBe(usersInDb[0].Id);
            model.ElementAt(1).Id.ShouldBe(usersInDb[1].Id);
            model.ElementAt(0).Roles.Count.ShouldBe(usersInDb[0].Roles.Count);
            model.ElementAt(0).Roles.Count.ShouldBe(usersInDb[1].Roles.Count);
        }
    }
}
