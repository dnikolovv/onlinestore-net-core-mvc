namespace OnlineStore.Infrastructure.Authorization.Requirements
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using OnlineStore.Infrastructure.Constants;

    public class HasClaimOrIsAdminHandler : AuthorizationHandler<HasClaimOrIsAdminRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasClaimOrIsAdminRequirement requirement)
        {
            if (context.User.IsInRole(Roles.ADMIN_ROLE))
            {
                context.Succeed(requirement);
            }

            return MustHaveClaim(context, requirement, requirement.ClaimType);
        }

        protected virtual Task MustHaveClaim(AuthorizationHandlerContext context, HasClaimOrIsAdminRequirement requirement, string claimType)
        {
            if (context.User.HasClaim(c => c.Type == claimType))
            {
                context.Succeed(requirement);
            }

            return Task.FromResult(context);
        }

    }
}
