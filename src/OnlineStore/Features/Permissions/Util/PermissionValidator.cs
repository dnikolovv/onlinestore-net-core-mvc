namespace OnlineStore.Features.Permissions.Util
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using System.Threading;
    using System.Threading.Tasks;

    public class PermissionValidator : IPermissionValidator
    {
        public PermissionValidator(ApplicationDbContext db)
        {
            this.db = db;
        }

        private readonly ApplicationDbContext db;

        public async Task<bool> PermissionDoesntExistAsync(string controller, string action, CancellationToken cancToken)
        {
            var permissionInDb = await this.db.Permissions
                .FirstOrDefaultAsync(p => p.Controller == controller.ToUpper() && p.Action.ToUpper() == action);

            return permissionInDb == null;
        }
    }
}
