namespace OnlineStore.Features.Admin
{
    using AutoMapper;
    using Data;
    using Infrastructure.ViewModels.Orders;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Orders
    {
        public class Query : IAsyncRequest<IEnumerable<OrderViewModel>> { }

        public class QueryHandler : IAsyncRequestHandler<Query, IEnumerable<OrderViewModel>>
        {
            public QueryHandler(ApplicationDbContext db)
            {
                this.db = db;
            }

            private ApplicationDbContext db;

            public async Task<IEnumerable<OrderViewModel>> Handle(Query message)
            {
                var viewModels = new List<OrderViewModel>();

                var unshippedOrders = await this.db.Orders
                    .Where(o => !o.Shipped)
                    .Include(o => o.OrderedItems)
                        .ThenInclude(i => i.Product)
                    .ToListAsync();

                Mapper.Map(unshippedOrders, viewModels);

                return viewModels;
            }
        }
    }
}
