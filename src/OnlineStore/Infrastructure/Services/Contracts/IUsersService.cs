namespace OnlineStore.Infrastructure.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    public interface IUsersService 
    {
        Task<bool> LoginAsync(string userName, string password);

        Task<bool> RegisterAsync(User user, string password);

        Task<User> FindByUserNameAsync(string userName);

        Task<IEnumerable<UserRole>> GetRolesAsync(int userId);

        Task<IEnumerable<IdentityUserClaim<int>>> GetClaimsAsync(int userId);

        Task<bool> UpdateAsync(User user, IEnumerable<string> newRoles);

        Task UpdateRolesAsync(User user, IEnumerable<string> newRoles);

        Task LogoutAsync();
    }
}
