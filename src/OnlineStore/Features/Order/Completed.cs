namespace OnlineStore.Features.Order
{
    using Data;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class Completed
    {
        public class Command : IAsyncRequest
        {
            public string CurrentUserName { get; set; }
        }

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            public CommandHandler(ApplicationDbContext db)
            {
                this.db = db;
            }

            private readonly ApplicationDbContext db;

            protected override async Task HandleCore(Command message)
            {
                var currentUserCart = await db
                    .Carts
                    .Include(c => c.OrderedItems)
                    .FirstOrDefaultAsync(c => c.User.UserName == message.CurrentUserName);

                currentUserCart.OrderedItems.Clear();
            }
        }
    }
}
