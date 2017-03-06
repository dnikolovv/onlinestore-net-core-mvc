namespace OnlineStore.Infrastructure.ViewModels.Users
{
    using System.Collections.Generic;

    public class UserEditViewModel
    {
        public ICollection<RoleViewModel> AvailableRoles { get; set; }

        public UserViewModel User { get; set; }
    }
}
