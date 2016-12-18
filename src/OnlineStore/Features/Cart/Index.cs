namespace OnlineStore.Features.Cart
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Infrastructure.ViewModels.Cart;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class Index
    {
        public class Query : IAsyncRequest<CartIndexViewModel>
        {
            public string CurrentUserName { get; set; }

            public string ReturnUrl { get; set; }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, CartIndexViewModel>
        {
            public QueryHandler(ApplicationDbContext db)
            {
                this.db = db;
            }

            private readonly ApplicationDbContext db;

            public async Task<CartIndexViewModel> Handle(Query message)
            {
                var currentUserCart = await this.db.Carts
                    .Where(c => c.User.UserName == message.CurrentUserName)
                        .Include(c => c.OrderedItems)
                            .ThenInclude(i => i.Product)
                                .ThenInclude(p => p.Category)
                    .FirstOrDefaultAsync();

                var orderedItems = currentUserCart
                    .OrderedItems
                    .AsQueryable()
                    .ProjectTo<CartItemViewModel>()
                    .ToList();

                return new CartIndexViewModel
                {
                    OrderedItems = orderedItems,
                    ReturnUrl = message.ReturnUrl,
                    TotalSum = currentUserCart.TotalSum()
                };
            }
        }
    }
}
