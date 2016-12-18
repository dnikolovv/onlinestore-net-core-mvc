namespace OnlineStore.Features.Admin
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Infrastructure.ViewModels.Products;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Products
    {
        public class Query : IAsyncRequest<IEnumerable<ProductViewModel>> { }

        public class QueryHandler : IAsyncRequestHandler<Query, IEnumerable<ProductViewModel>>
        {
            public QueryHandler(ApplicationDbContext db)
            {
                this.db = db;
            }

            private readonly ApplicationDbContext db;

            public async Task<IEnumerable<ProductViewModel>> Handle(Query message)
            {
                return await this.db.Products
                    .ProjectTo<ProductViewModel>()
                    .OrderByDescending(p => p.DateAdded)
                    .ToListAsync();
            }
        }
    }
}
