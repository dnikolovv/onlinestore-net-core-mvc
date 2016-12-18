namespace OnlineStore.Infrastructure.Attributes
{
    using Features;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// Used for redirecting the unauthorized user to the login page after an ajax request.
    /// </summary>
    public class JsonAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = (context.Controller as Controller)
                    .RedirectToActionJson("Login", "Account");
            }
        }
    }
}
