namespace OnlineStore.Infrastructure.Authorization
{
    using Microsoft.AspNetCore.Authorization;

    public sealed class AuthorizationSection
    {
        public AuthorizationSection(
            string policyName,
            string claimName,
            string friendlyName,
            bool requiresAdminPermissions,
            IAuthorizationRequirement requirement,
            IAuthorizationHandler handler)
        {
            this.PolicyName = policyName;
            this.ClaimType = claimName;
            this.FriendlyName = friendlyName;
            this.RequiresAdminPermissions = requiresAdminPermissions;
            this.Requirement = requirement;
            this.Handler = handler;
        }

        public string PolicyName { get; }

        public string ClaimType { get; set; }

        public string FriendlyName { get; set; }

        public bool RequiresAdminPermissions { get; set; }

        public IAuthorizationRequirement Requirement { get; }

        public IAuthorizationHandler Handler { get; }
    }
}
