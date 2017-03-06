namespace OnlineStore.IntegrationTests.Features.Users
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using OnlineStore.Features.Account;
    using OnlineStore.Features.Users;
    using System;
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
        }
    }
}
