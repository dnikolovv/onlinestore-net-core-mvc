namespace OnlineStore.Infrastructure.Authorization.Requirements
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using OnlineStore.Infrastructure.Constants;

    /// <summary>
    /// All custom requirement handlers should implement this abstract class.
    /// It ensures that if the Admin tries to request something that is policy authorized, he will always be let in.
    /// </summary>
    /// <typeparam name="T">The requirement type.</typeparam>
    public abstract class CustomRequirementHandler<T> : AuthorizationHandler<T>
        where T : IAuthorizationRequirement
    {
        protected abstract Task HandleCustomRequirementAsync(AuthorizationHandlerContext context, T requirement);

        protected virtual Task MustHaveClaim(AuthorizationHandlerContext context, T requirement, string claimType)
        {
            if (context.User.HasClaim(c => c.Type == claimType))
            {
                context.Succeed(requirement);
            }

            return Task.FromResult(context);
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, T requirement)
        {
            if (context.User.IsInRole(Roles.ADMIN_ROLE))
            {
                context.Succeed(requirement);
            }

            return HandleCustomRequirementAsync(context, requirement);
        }
    }
}
