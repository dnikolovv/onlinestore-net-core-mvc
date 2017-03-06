namespace OnlineStore.Data.Models
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    public class UserRole : IdentityRole<int>, IEntity
    {
    }
}
