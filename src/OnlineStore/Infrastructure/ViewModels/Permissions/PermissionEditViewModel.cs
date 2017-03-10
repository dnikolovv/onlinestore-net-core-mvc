namespace OnlineStore.Infrastructure.ViewModels.Permissions
{
    using System;
    using System.Collections.Generic;
    using Users;

    public class PermissionEditViewModel
    {
        public int Id { get; set; }

        public string Action { get; set; }

        public string Controller { get; set; }

        public ICollection<RoleViewModel> AvailableRoles { get; set; }

        public ICollection<int> SelectedRoles { get; set; }
    }
}
