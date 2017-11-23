namespace OnlineStore.Infrastructure.Services.Concrete
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts;
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class UsersService : IUsersService
    {
        public UsersService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<UserRole> roleManager, ApplicationDbContext db)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.db = db;
        }

        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<UserRole> roleManager;
        private readonly ApplicationDbContext db;

        public async Task<bool> LoginAsync(string userName, string password)
        {
            User user = await userManager.FindByNameAsync(userName);

            if (user != null)
            {
                SignInResult signIn = await signInManager.PasswordSignInAsync(user, password, false, false);

                if (signIn.Succeeded)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> UpdateAsync(User user, IEnumerable<string> newRoles = null)
        {
            if (user != null)
            {
                await UpdateRolesAsync(user, newRoles);
                IdentityResult update = await this.userManager.UpdateAsync(user);

                if (update.Succeeded)
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

        public async Task<IEnumerable<UserRole>> GetRolesAsync(int userId)
        {
            var roles = await this.db
                .Roles
                .Include(r => r.Users)
                .Where(r => r.Users.Select(u => u.UserId).Contains(userId))
                .ToListAsync();

            return roles;
        }

        public async Task UpdateRolesAsync(User user, IEnumerable<string> newRoles)
        {
            await userManager.RemoveFromRolesAsync(user, await userManager.GetRolesAsync(user));

            if (newRoles != null)
            {
                foreach (var roleName in newRoles)
                {
                    if (await roleManager.RoleExistsAsync(roleName))
                    {
                        await userManager.AddToRoleAsync(user, roleName);
                    }
                } 
            }
        }

        public async Task<IEnumerable<IdentityUserClaim<int>>> GetClaimsAsync(int userId)
        {
            var claims = await this.db
                .UserClaims
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return claims;
        }
    }
}
