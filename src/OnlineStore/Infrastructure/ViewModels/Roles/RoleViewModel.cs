namespace OnlineStore.Infrastructure.ViewModels.Roles
{
    using System.Collections.Generic;

    public class RoleViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<string> Claims { get; set; }
    }
}
