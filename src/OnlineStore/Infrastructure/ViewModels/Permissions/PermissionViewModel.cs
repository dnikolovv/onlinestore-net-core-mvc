namespace OnlineStore.Infrastructure.ViewModels.Permissions
{
    public class PermissionViewModel
    {
        public int Id { get; set; }

        public string Action { get; set; }

        public string Controller { get; set; }

        public int PermissionsRolesCount { get; set; }
    }
}
