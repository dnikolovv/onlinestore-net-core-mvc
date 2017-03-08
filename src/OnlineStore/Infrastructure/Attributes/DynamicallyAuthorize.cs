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
    public class DynamicallyAuthorize : ActionFilterAttribute
    {
        public DynamicallyAuthorize(ApplicationDbContext db)
        {
            this.db = db;
        }

        private readonly ApplicationDbContext db;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Check if user is admin
            bool userIsAuthorized = context.HttpContext.User.IsInRole(Constants.Roles.ADMIN_ROLE);
            
            // If he isn't go for the database
            if (!userIsAuthorized)
            {
                string actionName = (string)context.RouteData.Values["action"];
                string controllerName = (string)context.RouteData.Values["controller"];

                var permissions = this.db.Permissions
                        .Include(p => p.PermissionsRoles)   
                            .ThenInclude(pr => pr.Role)
                        .Where(p => p.Action == actionName.ToLower() && p.Controller == controllerName.ToLower())
                        .ToList();

                // If any explicitly stated permissions were found in the database
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
            }

            if (!userIsAuthorized)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}
