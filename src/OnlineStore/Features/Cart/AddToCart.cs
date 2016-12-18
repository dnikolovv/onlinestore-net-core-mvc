namespace OnlineStore.Features.Cart
{
    using Data;
    using Data.Models;
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class AddToCart
    {
        public class Command : IAsyncRequest
        {
            public string CurrentUserName { get; set; }

            public int ProductId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(q => q.ProductId).NotEqual(0);
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
                var currentUserCart = await this.db.Carts
                    .Include(c => c.OrderedItems)
                        .ThenInclude(c => c.Product)
                    .FirstOrDefaultAsync(c => c.User.UserName == message.CurrentUserName);

                var productInCart = currentUserCart.OrderedItems
                    .FirstOrDefault(oi => oi.Product.Id == message.ProductId);

                if (productInCart != null)
                {
                    productInCart.Quantity++;
                }
                else
                {
                    var productInDb = await this.db.Products.FirstOrDefaultAsync(p => p.Id == message.ProductId);
                    currentUserCart.OrderedItems.Add(new CartItem() { Product = productInDb, Quantity = 1 });
                }
            }
        }
    }
}
