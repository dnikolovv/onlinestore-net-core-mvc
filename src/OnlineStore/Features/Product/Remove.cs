namespace OnlineStore.Features.Product
{
    using Data;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class Remove
    {
        public class Command : IAsyncRequest
        {
            public int ProductId { get; set; }
        }

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            public CommandHandler(ApplicationDbContext db)
            {
                this.db = db;
            }

            private readonly ApplicationDbContext db;

            protected async override Task HandleCore(Command message)
            {
                var product = await this.db.Products.FirstOrDefaultAsync(p => p.Id == message.ProductId);
                this.db.Products.Remove(product);
                await this.db.SaveChangesAsync();
            }
        }
    }
}
