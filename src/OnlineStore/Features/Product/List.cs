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
    using ViewModels;

    public class List
    {
        public class Query : IAsyncRequest<Result>
        {
            public string Category { get; set; }

            public int Page { get; set; }

            public int PageSize { get; set; }
        }

        public class Result
        {
            public IEnumerable<ProductViewModel> Products { get; set; }

            public PagingInfo PagingInfo { get; set; }

            public string CurrentCategory { get; set; }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, Result>
        {
            public QueryHandler(ApplicationDbContext db)
            {
                this.db = db;
            }

            private readonly ApplicationDbContext db;

            public async Task<Result> Handle(Query message)
            {
                var productsQueryable = this.db.Products
                    .OrderByDescending(p => p.DateAdded)
                    .Where(p => message.Category == null ? true : p.Category.Name == message.Category);

                PagingInfo pagingInfo = new PagingInfo()
                {
                    CurrentPage = message.Page,
                    ItemsPerPage = (int)message.PageSize,
                    TotalItems = productsQueryable.Count()
                };

                var viewModels = await productsQueryable
                    .ProjectTo<ProductViewModel>()
                    .Skip((message.Page - 1) * (int)message.PageSize)
                    .Take((int)message.PageSize)
                    .ToListAsync();

                return new Result
                {
                    Products = viewModels,
                    PagingInfo = pagingInfo,
                    CurrentCategory = message.Category
                };
            }
        }
    }
}
