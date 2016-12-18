namespace OnlineStore.Features.Admin
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Infrastructure.ViewModels.Categories;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class Categories
    {
        public class Query : IAsyncRequest<IEnumerable<CategoryViewModel>> { }

        public class QueryHandler : IAsyncRequestHandler<Query, IEnumerable<CategoryViewModel>>
        {
            public QueryHandler(ApplicationDbContext db)
            {
                this.db = db;
            }

            private readonly ApplicationDbContext db;

            public async Task<IEnumerable<CategoryViewModel>> Handle(Query message)
            {
                return await this.db.Categories
                    .ProjectTo<CategoryViewModel>()
                    .ToListAsync();
            }
        }
    }
}
