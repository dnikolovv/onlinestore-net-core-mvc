namespace OnlineStore.UnitTests.Infrastructure.Attributes
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
    using Xunit;

    public class DynamicallyAuthorizeServiceFilterTests
    {
        [Fact]
        public void ReturnsUnauthorizedResultIfUserDoesntHavePermission()
        {
            // Arrange
            var db = GetInMemoryDb();

            var serviceFilter = new DynamicallyAuthorizeServiceFilter(db);
            var filterContext = ActionExecutingContextProvider.GetActionExecutingContext("GET");

            filterContext.HttpContext.User = A.Fake<ClaimsPrincipal>();
            filterContext.RouteData.Values["action"] = "This";
            filterContext.RouteData.Values["controller"] = "DoesntReallyMatterButItsNeeded";

            // Act
            serviceFilter.OnActionExecuting(filterContext);

            // Assert
            filterContext.Result.ShouldBeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task RegularUserWithPermissionIsLetThrough()
        {
            // Arrange
            var db = GetInMemoryDb();
            string actionToTestOn = "TestAction";
            string controllerToTestOn = "TestController";
            var role = await AddSampleRoleToDatabase(db, actionToTestOn, controllerToTestOn);

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
                var context = ActionExecutingContextProvider.GetActionExecutingContext("GET");
                
                context.RouteData.Values["action"] = actionToTestOn;
                context.RouteData.Values["controller"] = controllerToTestOn;

                var user = A.Fake<ClaimsPrincipal>();
                A.CallTo(() => user.IsInRole(role.Name)).Returns(true);

                context.HttpContext.User = user;

                return context;
            }
        }

        [Fact]
        public void AdminUserIsLetThrough()
        {
            // Arrange
            var db = GetInMemoryDb();
            var serviceFilter = new DynamicallyAuthorizeServiceFilter(db);
            var filterContext = ActionExecutingContextProvider.GetActionExecutingContext("GET");

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
        }
        
        private async Task<UserRole> AddSampleRoleToDatabase(ApplicationDbContext db, string permissionedAction, string permissionedController)
        {
            var permission = new Permission
            {
                Action = permissionedAction.ToLower(),
                Controller = permissionedController.ToLower()
            };

            db.Permissions.Add(permission);
            await db.SaveChangesAsync();
            db.Attach(permission);

            var role = new UserRole
            {
                Name = "SampleRole",
                PermissionsRoles = new List<PermissionRole>
                {
                    new PermissionRole() { Permission = permission, PermissionId = permission.Id }
                }
            };

            db.Roles.Add(role);
            await db.SaveChangesAsync();
            db.Roles.Attach(role);

            return role;
        }

        private static ApplicationDbContext GetInMemoryDb()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            var db = new ApplicationDbContext(optionsBuilder.Options);
            return db;
        }
    }
}
