namespace OnlineStore.Features.Admin
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Infrastructure.ViewModels.Permissions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class Permissions
    {
        public class Query : IAsyncRequest<IEnumerable<PermissionViewModel>> { }

        public class QueryHandler : IAsyncRequestHandler<Query, IEnumerable<PermissionViewModel>>
        {
            public QueryHandler(ApplicationDbContext db)
            {
                this.db = db;
            }

            private readonly ApplicationDbContext db;

            public async Task<IEnumerable<PermissionViewModel>> Handle(Query message)
            {
                var permissions = this.db.Permissions
                    .Include(p => p.PermissionsRoles);

                var viewModels = await permissions
                    .ProjectTo<PermissionViewModel>()
                    .ToListAsync();

                return viewModels;
            }
        }
    }
}
