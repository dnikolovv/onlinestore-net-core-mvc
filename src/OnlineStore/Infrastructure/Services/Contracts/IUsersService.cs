namespace OnlineStore.Infrastructure.Services.Contracts
{
    using Data.Models;
    using System.Threading.Tasks;

    public interface IUsersService 
    {
        Task<bool> LoginAsync(string userName, string password);

        Task<bool> RegisterAsync(User user, string password);

        Task<User> FindByUserNameAsync(string userName);

        Task LogoutAsync();
    }
}
