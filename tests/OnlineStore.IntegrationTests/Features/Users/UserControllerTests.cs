namespace OnlineStore.IntegrationTests.Features.Users
{
    using Microsoft.EntityFrameworkCore;
    using OnlineStore.Data.Models;
    using OnlineStore.Features.Account;
    using OnlineStore.Features.Users;
    using OnlineStore.Infrastructure.Constants;
    using OnlineStore.IntegrationTests.Extensions;
    using System.Threading.Tasks;

    public class UserControllerTests
    {
        public async Task SuccessfullEditSetsSuccessMessage(SliceFixture fixture)
        {
            // Arrange
            var controller = fixture.InstantiateController<UserController>();
            var user = AddUserToDb();

            // Act
            var editCommand = new Edit.Command
            {
                Id = user.Id,
                UserName = "editedUserName"
            };

            await controller.Edit(editCommand);

            // Assert
            controller.TempData
                .ShouldContainSuccessMessage(SuccessMessages.SuccessfullyEditedUser(editCommand.UserName));

            async Task<User> AddUserToDb()
            {
                var registerUserCommand = new Register.Command
                {
                    FirstName = "John",
                    UserName = "user",
                    Password = "password123"
                };
                
                await fixture.SendAsync(registerUserCommand);

                return await fixture.ExecuteDbContextAsync(db => db
                    .Users
                    .FirstOrDefaultAsync(u => u.UserName == registerUserCommand.UserName));
            }
        }
    }
}
