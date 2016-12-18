namespace OnlineStore.Features.Cart
{
    using Data;
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class RemoveFromCart
    {
        public class Command : IAsyncRequest
        {
            public int ProductId { get; set; }

            public string CurrentUserName { get; set; }

            public string ReturnUrl { get; set; }
        }

        public class QueryValidator : AbstractValidator<Command>
        {
            public QueryValidator()
            {
                RuleFor(q => q.ProductId).NotNull();
            }
        }

        public class QueryHandler : AsyncRequestHandler<Command>
        {
            public QueryHandler(ApplicationDbContext db)
            {
                this.db = db;
            }

            private readonly ApplicationDbContext db;

            protected override async Task HandleCore(Command message)
            {
                var currentUserCart = await this.db.Carts
                    .Include(c => c.OrderedItems)
                        .ThenInclude(c => c.Product)
                    .FirstOrDefaultAsync(c => c.User.UserName == message.CurrentUserName);

                var productInCart = currentUserCart.OrderedItems
                    .FirstOrDefault(oi => oi.Product.Id == message.ProductId);

                if (productInCart != null)
                {
                    currentUserCart.OrderedItems.Remove(productInCart);
                }
            }
        }
    }
}
