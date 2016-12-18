namespace OnlineStore.Features.Product
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Infrastructure.ViewModels.Products;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ListLatest
    {
        public class Query : IAsyncRequest<IEnumerable<ProductViewModel>>
        {
            public int NumberOfItems { get; set; }
        }

        public class QueryHandlder : IAsyncRequestHandler<Query, IEnumerable<ProductViewModel>>
        {
            public QueryHandlder(ApplicationDbContext db)
            {
                this.db = db;
            }

            private readonly ApplicationDbContext db;

            public async Task<IEnumerable<ProductViewModel>> Handle(Query message)
            {
                var productsQueryable = this.db.Products
                    .OrderByDescending(p => p.DateAdded);

                var viewModels = await productsQueryable
                    .ProjectTo<ProductViewModel>()
                    .Take(message.NumberOfItems)
                    .ToListAsync();

                return viewModels;
            }
        }
    }
}
