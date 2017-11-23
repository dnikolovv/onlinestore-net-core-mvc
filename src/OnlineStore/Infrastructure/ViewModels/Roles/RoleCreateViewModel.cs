namespace OnlineStore.Infrastructure.ViewModels.Roles
{
    using System.Collections.Generic;

    public class RoleCreateViewModel
    {
        public string Name { get; set; }

        public ICollection<AuthorizationSectionViewModel> AvailableSections { get; set; }

        public ICollection<string> SelectedClaims { get; set; }
    }
}
