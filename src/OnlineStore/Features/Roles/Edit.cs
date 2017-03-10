namespace OnlineStore.Features.Roles
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using FluentValidation;
    using Infrastructure.ViewModels.Permissions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using OnlineStore.Infrastructure.ViewModels.Roles;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Edit
    {
        public class Query : IAsyncRequest<RoleEditViewModel>
        {
            public int? RoleId { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.RoleId).NotNull();
            }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, RoleEditViewModel>
        {
            public QueryHandler(ApplicationDbContext db)
            {
                this.db = db;
            }

            private readonly ApplicationDbContext db;

            public async Task<RoleEditViewModel> Handle(Query message)
            {

                var roleInDb = await this.db.Roles
                    .Include(r => r.PermissionsRoles)
                    .FirstOrDefaultAsync(r => r.Id == message.RoleId);

                var availablePermissions = await this.db.Permissions
                    .ProjectTo<PermissionViewModel>()
                    .ToListAsync();

                return new RoleEditViewModel
                {
                    Id = roleInDb.Id,
                    Name = roleInDb.Name,
                    AvailablePermissions = availablePermissions,
                    SelectedPermissions = roleInDb.PermissionsRoles.Select(p => p.PermissionId).ToList()
                };
            }
        }

        public class Command : IAsyncRequest
        {
            public int? Id { get; set; }

            public string Name { get; set; }

            /// <summary>
            /// Holds permission id's
            /// </summary>
            public ICollection<int> SelectedPermissions { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Id).NotNull();
                RuleFor(c => c.Name).NotNull().NotEmpty();
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
                var roleInDb = await this.db.Roles
                    .Include(r => r.PermissionsRoles)
                    .FirstOrDefaultAsync(r => r.Id == message.Id);

                Mapper.Map(message, roleInDb);
                await UpdatePermissions(roleInDb, message.SelectedPermissions);
            }

            // TODO: This is a duplicate of the one found in Create
            private async Task UpdatePermissions(UserRole role, ICollection<int> permissionIds)
            {
                if (permissionIds != null && permissionIds.Count > 0)
                {
                    foreach (var permissionId in permissionIds)
                    {
                        var permissionInDb = await this.db.Permissions
                            .FirstOrDefaultAsync(p => p.Id == permissionId);

                        if (permissionInDb != null)
                        {
                            bool relationshipAlreadyExists = role.PermissionsRoles?
                                .FirstOrDefault(pr => pr.PermissionId == permissionInDb.Id && pr.RoleId == role.Id) != null;

                            if (!relationshipAlreadyExists)
                            {
                                PermissionRole relationship = new PermissionRole { RoleId = role.Id, PermissionId = permissionInDb.Id };
                                this.db.PermissionsRoles.Add(relationship);
                            }
                        }
                    }
                }
                else
                {
                    role.PermissionsRoles?.Clear();
                }
            }
        }
    }
}
