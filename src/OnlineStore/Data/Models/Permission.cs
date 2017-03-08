namespace OnlineStore.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Permission : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Action { get; set; }

        [Required]
        public string Controller { get; set; }

        public ICollection<PermissionRole> PermissionsRoles { get; set; }
    }
}
