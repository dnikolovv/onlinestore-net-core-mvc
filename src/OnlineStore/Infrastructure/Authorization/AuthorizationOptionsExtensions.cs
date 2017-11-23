namespace OnlineStore.Infrastructure.Authorization
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Authorization;

    public static class AuthorizationOptionsExtensions
    {
        public static void RegisterAuthorizationSections(
            this AuthorizationOptions options, 
            IEnumerable<AuthorizationSection> sections)
        {
            foreach (var section in sections)
            {
                options.AddPolicy(
                    section.PolicyName,
                    policy => policy.Requirements.Add(section.Requirement));
            }
        }
    }
}
