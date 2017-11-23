namespace OnlineStore.Features.Admin
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using Data;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using OnlineStore.Infrastructure.ViewModels.Roles;

    public class Roles
    {
        public class Query : IAsyncRequest<IEnumerable<RoleViewModel>> { }

        public class QueryHandler : IAsyncRequestHandler<Query, IEnumerable<RoleViewModel>>
        {
            public QueryHandler(ApplicationDbContext db)
            {
                this.db = db;
            }

            private readonly ApplicationDbContext db;

            public async Task<IEnumerable<RoleViewModel>> Handle(Query message)
            {
                return await this.db.Roles
                    .ProjectTo<RoleViewModel>()
                    .ToListAsync();
            }
        }
    }
}
