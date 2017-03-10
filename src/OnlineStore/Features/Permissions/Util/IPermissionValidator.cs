namespace OnlineStore.Features.Permissions.Util
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IPermissionValidator
    {
        Task<bool> PermissionDoesntExistAsync(string controllerName, string actionName, CancellationToken cancToken);
    }
}
