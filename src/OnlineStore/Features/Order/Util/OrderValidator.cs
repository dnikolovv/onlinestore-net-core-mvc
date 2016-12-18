namespace OnlineStore.Features.Order.Util
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class OrderValidator : IOrderValidator
    {
        public OrderValidator(ApplicationDbContext db)
        {
            this.db = db;
        }

        private readonly ApplicationDbContext db;

        public async Task<bool> UserHasItemsInCartAsync(string userName, CancellationToken cancellationToken)
        {
            var user = await this.db.Users
                .Include(u => u.Cart)
                    .ThenInclude(c => c.OrderedItems)
                .FirstOrDefaultAsync(u => u.UserName == userName);

            return user.Cart.OrderedItems.Any();
        }
    }
}
