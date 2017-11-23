namespace OnlineStore.Infrastructure.ViewModels.Users
{
    using System.Collections.Generic;
    using OnlineStore.Infrastructure.ViewModels.Roles;

    public class UserViewModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public ICollection<RoleViewModel> Roles { get; set; }
    }
}
