namespace OnlineStore.Infrastructure.Services.Concrete
{
    using Contracts;
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using System.Threading.Tasks;

    public class UsersService : IUsersService
    {
        public UsersService(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext db)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.db = db;
        }

        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ApplicationDbContext db;

        public async Task<bool> LoginAsync(string userName, string password)
        {
            User user = await userManager.FindByNameAsync(userName);

            if (user != null)
            {
                // Make sure the user is not logged when attempting to do so
                await signInManager.SignOutAsync();

                SignInResult signIn = await signInManager.PasswordSignInAsync(user, password, false, false);

                if (signIn.Succeeded)
                {
                    return true;
                }
            }
            
            return false;
        }

        public async Task<bool> RegisterAsync(User user, string password)
        {
            IdentityResult register = await userManager.CreateAsync(user, password);

            if (register.Succeeded)
            {
                Cart cart = new Cart() { User = user, UserId = user.Id };
                db.Carts.Add(cart);
                user.Cart = cart;
                return true;
            }

            return false;
        }

        public async Task LogoutAsync()
        {
            await this.signInManager.SignOutAsync();
        }

        public async Task<User> FindByUserNameAsync(string userName)
        {
            return await this.userManager.FindByNameAsync(userName);
        }
    }
}
