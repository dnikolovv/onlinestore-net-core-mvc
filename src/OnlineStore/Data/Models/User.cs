namespace OnlineStore.Data.Models
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int CartId { get; set; }

        public Cart Cart { get; set; }
    }
}
