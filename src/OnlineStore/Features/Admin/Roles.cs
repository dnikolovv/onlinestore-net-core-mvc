namespace OnlineStore.Features.Admin
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Infrastructure.ViewModels.Users;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

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
