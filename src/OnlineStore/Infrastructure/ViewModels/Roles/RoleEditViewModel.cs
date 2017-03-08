namespace OnlineStore.Infrastructure.ViewModels.Roles
{
    using Permissions;
    using System.Collections.Generic;

    public class RoleEditViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<PermissionViewModel> AvailablePermissions { get; set; }

        public ICollection<int> SelectedPermissions { get; set; }
    }
}
