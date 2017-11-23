namespace OnlineStore.Infrastructure.Authorization.Requirements
{
    using Microsoft.AspNetCore.Authorization;

    public class HasClaimOrIsAdminRequirement : IAuthorizationRequirement
    {
        public HasClaimOrIsAdminRequirement(string claimType)
        {
            this.ClaimType = claimType;
        }

        public string ClaimType { get; }
    }
}
