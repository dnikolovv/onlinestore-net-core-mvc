namespace OnlineStore.IntegrationTests.Infrastructure.Services
{
    using FakeItEasy;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using OnlineStore.Data;
    using OnlineStore.Data.Models;
    using OnlineStore.Features.Account;
    using OnlineStore.Infrastructure.Services.Concrete;
    using Shouldly;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UsersServiceTests
    {
        public async Task CantLoginWithInvalidAccount(SliceFixture fixture)
        {
            await fixture.ExecuteScopeAsync(async sp =>
            {
                await fixture.ExecuteDbContextAsync(async db =>
                {
                    // Arrange
                    var userManager = GetFakeUserManager(sp);
                    var usersService = new UsersService(userManager, GetFakeSignInManager(userManager), db);

                    // Act
                    var result = await usersService.LoginAsync("unexisting", "user");

                    // Assert
                    result.ShouldBeFalse();
                });
            });
        }

        public async Task CanLogin(SliceFixture fixture)
        {
            await fixture.ExecuteScopeAsync(async sp =>
            {
                await fixture.ExecuteDbContextAsync(async db =>
                {
                    // Arrange
                    var registerUserCommand = new Register.Command
                    {
                        UserName = "user1",
                        Password = "securePassword"
                    };

                    await fixture.SendAsync(registerUserCommand);

                    var user = await db
                        .Users
                        .FirstOrDefaultAsync(u => u.UserName == registerUserCommand.UserName);

                    var userManager = GetFakeUserManager(sp);
                    A.CallTo(() => userManager.FindByNameAsync(registerUserCommand.UserName)).Returns(user);

                    var signInManager = GetFakeSignInManager(userManager);

                    A.CallTo(() => signInManager.PasswordSignInAsync(user, registerUserCommand.Password, false, false))
                        .Returns(SignInResult.Success);

                    var usersService = new UsersService(userManager, signInManager, db);

                    // Act
                    var result = await usersService.LoginAsync(registerUserCommand.UserName, registerUserCommand.Password);

                    // Assert
                    result.ShouldBeTrue();
                });
            });
        }

        public async Task CanLogout(SliceFixture fixture)
        {
            await fixture.ExecuteScopeAsync(async sp =>
            {
                await fixture.ExecuteDbContextAsync(async db =>
                {
                    // Arrange
                    var userManager = GetFakeUserManager(sp);
                    var signInManager = GetFakeSignInManager(userManager);

                    var usersService = new UsersService(userManager, signInManager, db);

                    // Act
                    await usersService.LogoutAsync();

                    // Assert
                    A.CallTo(() => signInManager.SignOutAsync()).MustHaveHappened();
                });
            });
        }

        public async Task CanFindByUserName(SliceFixture fixture)
        {
            await fixture.ExecuteScopeAsync(async sp =>
            {
                await fixture.ExecuteDbContextAsync(async db =>
                {
                    // Arrange
                    var registerUserCommand = new Register.Command
                    {
                        UserName = "SomeUser",
                        Password = "password123",
                        FirstName = "John",
                        LastName = "Smith"
                    };

                    await fixture.SendAsync(registerUserCommand);

                    var expectedUser = await db
                        .Users
                        .FirstOrDefaultAsync(u => u.UserName == registerUserCommand.UserName);

                    var userManager = (UserManager<User>)sp.GetService(typeof(UserManager<User>));
                    var usersService = new UsersService(userManager, GetFakeSignInManager(userManager), db);

                    // Act
                    var actualUser = await usersService.FindByUserNameAsync(registerUserCommand.UserName);

                    // Assert
                    actualUser.UserName.ShouldBe(expectedUser.UserName);
                    actualUser.FirstName.ShouldBe(expectedUser.FirstName);
                    actualUser.LastName.ShouldBe(expectedUser.LastName);
                });
            });
        }

        public async Task CanGetRoles(SliceFixture fixture)
        {
            await fixture.ExecuteScopeAsync(async sp =>
            {
                await fixture.ExecuteDbContextAsync(async db =>
                {
                    // Arrange
                    var role = await AddSampleRoleToDatabaseAsync(fixture, db);

                    var registerUserCommand = new Register.Command
                    {
                        UserName = "SomeUser",
                        Password = "password123"
                    };

                    await fixture.SendAsync(registerUserCommand);

                    var user = await db
                        .Users
                        .FirstOrDefaultAsync(u => u.UserName == registerUserCommand.UserName);

                    await AssignRoleToUserAsync(fixture, role, user);

                    var userManager = (UserManager<User>)sp.GetService(typeof(UserManager<User>));
                    var usersService = new UsersService(userManager, GetFakeSignInManager(userManager), db);

                    // Act
                    var roles = await usersService.GetRolesAsync(user.Id.ToString());

                    // Assert
                    roles.Count().ShouldBe(1);
                    roles.First().Id.ShouldBe(role.Id);
                    roles.First().Name.ShouldBe(role.Name);
                });
            });
        }

        public async Task CanUpdate(SliceFixture fixture)
        {
            await fixture.ExecuteScopeAsync(async sp =>
            {
                await fixture.ExecuteDbContextAsync(async db =>
                {
                    // Arrange
                    var registerUserCommand = new Register.Command
                    {
                        UserName = "SomeUser",
                        Password = "Password123"
                    };

                    await fixture.SendAsync(registerUserCommand);

                    var user = await db
                        .Users
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => u.UserName == registerUserCommand.UserName);

                    var userManager = (UserManager<User>)sp.GetService(typeof(UserManager<User>));
                    var usersService = new UsersService(userManager, GetFakeSignInManager(userManager), db);

                    // Act
                    user.UserName = "updated";
                    var updateResult = await usersService.UpdateAsync(user);

                    // Assert
                    updateResult.ShouldBeTrue();

                    var userAfterUpdate = await usersService.FindByUserNameAsync(user.UserName);
                    userAfterUpdate.UserName.ShouldBe(user.UserName);
                });
            });
        }

        public async Task CanRegister(SliceFixture fixture)
        {
            await fixture.ExecuteScopeAsync(async sp =>
            {
                await fixture.ExecuteDbContextAsync(async db =>
                {
                    // Arrange
                    var userManager = (UserManager<User>)sp.GetService(typeof(UserManager<User>));
                    var usersService = new UsersService(userManager, GetFakeSignInManager(userManager), db);

                    var userToRegister = new User
                    {
                        FirstName = "Peter",
                        LastName = "McDonald",
                        UserName = "SomeUserName"
                    };

                    // Act
                    var registeredSuccessfully = await usersService.RegisterAsync(userToRegister, "somePassword123");

                    // Assert
                    registeredSuccessfully.ShouldBeTrue();

                    var registeredUser = await usersService.FindByUserNameAsync(userToRegister.UserName);

                    registeredUser.ShouldNotBeNull();
                    registeredUser.FirstName.ShouldBe(userToRegister.FirstName);
                    registeredUser.LastName.ShouldBe(userToRegister.LastName);
                });
            });
        }

        private static async Task AssignRoleToUserAsync(SliceFixture fixture, UserRole role, User user)
        {
            var assignRoleToUserCommand = new OnlineStore.Features.Users.Edit.Command
            {
                Id = user.Id,
                SelectedRoles = new[] { role.Id }
            };

            await fixture.SendAsync(assignRoleToUserCommand);
        }

        private static async Task<UserRole> AddSampleRoleToDatabaseAsync(SliceFixture fixture, ApplicationDbContext db)
        {
            var permission = new Permission
            {
                Action = "SomeAction",
                Controller = "SomeController"
            };

            await fixture.InsertAsync(permission);

            var createCommand = new OnlineStore.Features.Roles.Create.Command
            {
                Name = "SampleRole",
                SelectedPermissions = new List<int> { permission.Id }
            };

            await fixture.SendAsync(createCommand);

            return await db.Roles
                .Include(r => r.PermissionsRoles)
                    .ThenInclude(pr => pr.Permission)
                .FirstOrDefaultAsync(r => r.Name == createCommand.Name);
        }

        private static SignInManager<User> GetFakeSignInManager(UserManager<User> userManager)
        {
            return A.Fake<SignInManager<User>>(opts => opts
                .WithArgumentsForConstructor(() => new SignInManager<User>(
                    userManager,
                    A.Fake<IHttpContextAccessor>(),
                    A.Fake<IUserClaimsPrincipalFactory<User>>(),
                    A.Fake<IOptions<IdentityOptions>>(),
                    A.Fake<ILogger<SignInManager<User>>>())));
        }

        private static UserManager<User> GetFakeUserManager(System.IServiceProvider sp)
        {
            return A.Fake<UserManager<User>>(opts => opts
                .WithArgumentsForConstructor(() => new UserManager<User>(
                    A.Fake<IUserStore<User>>(),
                    A.Fake<IOptions<IdentityOptions>>(),
                    A.Fake<PasswordHasher<User>>(),
                    A.CollectionOfFake<IUserValidator<User>>(1),
                    A.CollectionOfFake<IPasswordValidator<User>>(1),
                    A.Fake<ILookupNormalizer>(),
                    A.Fake<IdentityErrorDescriber>(),
                    sp,
                    A.Fake<ILogger<UserManager<User>>>())));
        }
    }
}
