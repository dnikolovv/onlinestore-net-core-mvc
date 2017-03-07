namespace OnlineStore.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Permission : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Action { get; set; }

        [Required]
        public string Controller { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }

        public UserRole Role { get; set; }
    }
}
