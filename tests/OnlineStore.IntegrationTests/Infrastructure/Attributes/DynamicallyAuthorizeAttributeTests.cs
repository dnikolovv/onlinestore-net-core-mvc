namespace OnlineStore.IntegrationTests.Infrastructure.Attributes
{
    using FakeItEasy;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.EntityFrameworkCore;
    using OnlineStore.Data;
    using OnlineStore.Data.Models;
    using OnlineStore.Infrastructure.Attributes;
    using Shouldly;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class DynamicallyAuthorizeServiceFilterTests
    {
        public async Task ReturnsUnauthorizedResultIfUserDoesntHavePermission(SliceFixture fixture)
        {
            await fixture.ExecuteDbContextAsync(async db =>
            {
                await Task.Run(() =>
                {
                    // Arrange
                    var serviceFilter = new DynamicallyAuthorizeServiceFilter(db);
                    var filterContext = fixture.GetActionExecutingContext("GET");

                    filterContext.HttpContext.User = A.Fake<ClaimsPrincipal>();
                    filterContext.RouteData.Values["action"] = "This";
                    filterContext.RouteData.Values["controller"] = "DoesntReallyMatterButItsNeeded";

                    // Act
                    serviceFilter.OnActionExecuting(filterContext);

                    // Assert
                    filterContext.Result.ShouldBeOfType<UnauthorizedResult>();
                });
            });
        }

        public async Task RegularUserWithPermissionIsLetThrough(SliceFixture fixture)
        {
            await fixture.ExecuteDbContextAsync(async db =>
            {
                await Task.Run(async () =>
                {
                    // Arrange
                    string actionToTestOn = "TestAction";
                    string controllerToTestOn = "TestController";
                    var role = await AddSampleRoleToDatabase(fixture, db, actionToTestOn, controllerToTestOn);

                    var filterContext = SetupFilterContextWithUser();
                    var serviceFilter = new DynamicallyAuthorizeServiceFilter(db);

                    // Act
                    serviceFilter.OnActionExecuting(filterContext);

                    // Assert
                    A.CallToSet(() => filterContext.Result)
                        .To(() => A<IActionResult>
                            .That.IsInstanceOf(typeof(UnauthorizedResult)))
                    .MustNotHaveHappened();

                    ActionExecutingContext SetupFilterContextWithUser()
                    {
                        var context = fixture.GetActionExecutingContext("GET");

                        fixture.GetActionExecutingContext("GET");
                        context.RouteData.Values["action"] = actionToTestOn;
                        context.RouteData.Values["controller"] = controllerToTestOn;

                        var user = A.Fake<ClaimsPrincipal>();
                        A.CallTo(() => user.IsInRole(role.Name)).Returns(true);

                        context.HttpContext.User = user;

                        return context;
                    }
                });
            });
        }

        public async Task AdminUserIsLetThrough(SliceFixture fixture)
        {
            await fixture.ExecuteDbContextAsync(async db =>
            {
                await Task.Run(() =>
                {
                    // Arrange
                    var serviceFilter = new DynamicallyAuthorizeServiceFilter(db);
                    var filterContext = fixture.GetActionExecutingContext("GET");

                    var user = A.Fake<ClaimsPrincipal>();

                    A.CallTo(() => user.IsInRole(OnlineStore.Infrastructure.Constants.Roles.ADMIN_ROLE)).Returns(true);

                    filterContext.HttpContext.User = user;

                    // Act
                    serviceFilter.OnActionExecuting(filterContext);

                    // Assert
                    A.CallToSet(() => filterContext.Result)
                        .To(() => A<IActionResult>
                            .That
                            .IsInstanceOf(typeof(UnauthorizedResult)))
                    .MustNotHaveHappened();
                });
            });
        }

        private async Task<UserRole> AddSampleRoleToDatabase(SliceFixture fixture, ApplicationDbContext db, string permissionedAction, string permissionedController)
        {
            var permission = new Permission
            {
                Action = permissionedAction,
                Controller = permissionedController
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

    }
}
