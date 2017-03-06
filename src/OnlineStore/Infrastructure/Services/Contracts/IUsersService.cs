namespace OnlineStore.Infrastructure.Services.Contracts
{
    using Data.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUsersService 
    {
        Task<bool> LoginAsync(string userName, string password);

        Task<bool> RegisterAsync(User user, string password);

        Task<User> FindByIdAsync(string userId);

        Task<User> FindByUserNameAsync(string userName);

        Task<IEnumerable<UserRole>> GetRolesAsync(string userId);

        Task<bool> UpdateAsync(User user);

        Task LogoutAsync();
    }
}
