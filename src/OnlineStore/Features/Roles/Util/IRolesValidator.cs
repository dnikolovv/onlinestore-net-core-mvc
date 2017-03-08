namespace OnlineStore.Features.Roles.Util
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IRolesValidator
    {
        Task<bool> RoleNameNotTakenAsync(string roleName, CancellationToken cancToken);
    }
}
