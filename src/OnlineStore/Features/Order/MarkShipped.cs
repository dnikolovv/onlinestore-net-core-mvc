namespace OnlineStore.Features.Order
{
    using Data;
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class MarkShipped
    {
        public class Command : IAsyncRequest
        {
            public int OrderId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(q => q.OrderId).NotNull();
            }
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
                var orderInDb = await this.db.Orders
                    .FirstOrDefaultAsync(o => o.Id == message.OrderId);

                if (orderInDb != null)
                {
                    orderInDb.Shipped = true;
                }
            }
        }
    }
}
