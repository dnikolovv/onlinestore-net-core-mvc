﻿namespace OnlineStore.Infrastructure.ViewModels.Roles
{
    using System.Collections.Generic;

    public class RoleEditViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<AuthorizationSectionViewModel> AvailableSections { get; set; }

        public ICollection<string> SelectedClaims { get; set; }
    }
}
