namespace OnlineStore.Infrastructure.ViewModels.Roles
{
    using Permissions;
    using System.Collections.Generic;

    public class RoleCreateViewModel
    {
        public string Name { get; set; }

        public ICollection<PermissionViewModel> AvailablePermissions { get; set; }
    }
}
