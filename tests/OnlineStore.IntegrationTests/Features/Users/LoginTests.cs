namespace OnlineStore.IntegrationTests.Features.Users
{
    using OnlineStore.Features.Account;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class LoginTests
    {
        public async Task CanLogin(SliceFixture fixture)
        {
            // Arrange
            var loginCommand = new Login.Command
            {
                UserName = "SomeUser",
                Password = "password123"
            };

            // Act
            await fixture.SendAsync(loginCommand);
        }
    }
}
