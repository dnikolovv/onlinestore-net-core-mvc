namespace OnlineStore.Features.Roles.Util
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using System.Threading;
    using System.Threading.Tasks;

    public class RolesValidator : IRolesValidator
    {
        public RolesValidator(ApplicationDbContext db)
        {
            this.db = db;
        }

        private readonly ApplicationDbContext db;

        public async Task<bool> RoleNameNotTakenAsync(string categoryName, CancellationToken cancToken)
        {
            cancToken.ThrowIfCancellationRequested();

            var roleInDb = await this.db.Roles
                .FirstOrDefaultAsync(c => c.Name == categoryName);

            return roleInDb == null;
        }
    }
}
