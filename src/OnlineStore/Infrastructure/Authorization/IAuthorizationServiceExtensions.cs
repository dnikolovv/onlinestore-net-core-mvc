namespace OnlineStore.Infrastructure.Authorization
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;

    public static class IAuthorizationServiceExtensions
    {
        public static async Task<bool> IsAuthorizedForAdminPanelAsync(this IAuthorizationService authorization, ClaimsPrincipal user)
        {
            var isAdmin = user.IsInRole(Constants.Roles.ADMIN_ROLE);

            if (!isAdmin)
            {
                foreach (var section in AuthorizationSectionsContainer.Sections)
                {
                    return await authorization.AuthorizeAsync(user, section.PolicyName);
                }

                return false;
            }

            return true;
        }
    }
}
