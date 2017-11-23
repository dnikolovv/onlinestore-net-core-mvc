namespace OnlineStore.Infrastructure.ViewModels.Users
{
    using System.Collections.Generic;
    using OnlineStore.Infrastructure.ViewModels.Roles;

    public class UserEditViewModel
    {
        public ICollection<RoleViewModel> AvailableRoles { get; set; }

        public UserViewModel User { get; set; }
    }
}
