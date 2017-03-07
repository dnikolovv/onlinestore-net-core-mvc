namespace OnlineStore.IntegrationTests.Features.Users
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using OnlineStore.Features.Account;
    using OnlineStore.Features.Users;
    using Shouldly;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class EditTests
    {
        public async Task CanEdit(SliceFixture fixture)
        {
            // Arrange
            var registerUserCommand = new Register.Command
            {
                UserName = "User",
                FirstName = "Some name",
                Password = "password",
                ConfirmedPassword = "password"
            };

            await fixture.SendAsync(registerUserCommand);

            var role = new UserRole
            {
                Name = "Role1"
            };

            await fixture.InsertAsync(role);

            // Act
            var registeredUser = await fixture.ExecuteDbContextAsync(db => db
                .Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.UserName == registerUserCommand.UserName));

            registeredUser.Roles.Count.ShouldBe(0);

            var editCommand = new Edit.Command
            {
                Id = registeredUser.Id,
                SelectedRoles = new List<int> { role.Id }
            };

            await fixture.SendAsync(editCommand);
            
            // Assert
            var updatedUser = await fixture.ExecuteDbContextAsync(db => db
                .Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.UserName == registerUserCommand.UserName));

            updatedUser.Roles.Count.ShouldBe(1);
            updatedUser.Roles.ElementAt(0).RoleId.ShouldBe(role.Id);
        }

        public async Task QueryReturnsCorrectResults(SliceFixture fixture)
        {
            // Arrange
            // Register a user and add him a role
            var registerUserCommand = new Register.Command
            {
                UserName = "User",
                FirstName = "Some name",
                Password = "password",
                ConfirmedPassword = "password"
            };

            await fixture.SendAsync(registerUserCommand);

            var role = new UserRole
            {
                Name = "Role1"
            };

            await fixture.InsertAsync(role);

            var registeredUser = await fixture.ExecuteDbContextAsync(db => db
                .Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.UserName == registerUserCommand.UserName));

            var editCommand = new Edit.Command
            {
                Id = registeredUser.Id,
                SelectedRoles = new List<int> { role.Id }
            };

            await fixture.SendAsync(editCommand);

            // Act
            var query = new Edit.Query
            {
                UserId = registeredUser.Id
            };

            var response = await fixture.SendAsync(query);

            // Assert
            var updatedUser = await fixture.ExecuteDbContextAsync(db => db
                .Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.UserName == registerUserCommand.UserName));

            response.AvailableRoles.Count.ShouldBe(1);
            response.User.Id.ShouldBe(updatedUser.Id);
            response.User.UserName.ShouldBe(updatedUser.UserName);
            response.User.Roles.Count.ShouldBe(updatedUser.Roles.Count);
        }
    }
}
