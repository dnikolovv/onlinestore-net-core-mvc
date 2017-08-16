namespace OnlineStore.Infrastructure.Attributes
{
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;

    /// <summary>
    /// Used as a Service Filter.
    /// </summary>
    public class DynamicallyAuthorizeServiceFilter : ActionFilterAttribute
    {
        public DynamicallyAuthorizeServiceFilter(ApplicationDbContext db)
        {
            this.db = db;
        }

        private readonly ApplicationDbContext db;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool userIsAdmin = context.HttpContext.User.IsInRole(Constants.Roles.ADMIN_ROLE);

            if (!userIsAdmin && !UserIsAuthorized(context))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }

        private bool UserIsAuthorized(ActionExecutingContext context)
        {
            bool userIsAuthorized = false;

            string actionName = (string)context.RouteData.Values["action"];
            string controllerName = (string)context.RouteData.Values["controller"];

            var permissions = this.db.Permissions
                    .Include(p => p.PermissionsRoles)
                        .ThenInclude(pr => pr.Role)
                    .Where(p => p.Action == actionName.ToLower() && p.Controller == controllerName.ToLower())
                    .ToList();

            if (permissions != null && permissions.Count > 0)
            {
                foreach (var permission in permissions)
                {
                    foreach (var permissionRole in permission.PermissionsRoles)
                    {
                        if (userIsAuthorized) { break; }
                        userIsAuthorized = context.HttpContext.User.IsInRole(permissionRole.Role.Name);
                    }
                }
            }

            return userIsAuthorized;
        }
    }
}
