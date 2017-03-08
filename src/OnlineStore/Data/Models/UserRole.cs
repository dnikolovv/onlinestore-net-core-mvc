namespace OnlineStore.Data.Models
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using System.Collections.Generic;

    public class UserRole : IdentityRole<int>, IEntity
    {
        public ICollection<PermissionRole> PermissionsRoles { get; set; }
    }
}
