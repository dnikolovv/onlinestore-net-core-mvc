namespace OnlineStore.Features.Roles
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using FluentValidation;
    using Infrastructure.Constants;
    using Infrastructure.ViewModels.Permissions;
    using Infrastructure.ViewModels.Roles;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Util;

    public class Create
    {
        public class Query : IAsyncRequest<RoleCreateViewModel> { }

        public class QueryHandler : IAsyncRequestHandler<Query, RoleCreateViewModel>
        {
            public QueryHandler(ApplicationDbContext db)
            {
                this.db = db;
            }

            private readonly ApplicationDbContext db;

            public async Task<RoleCreateViewModel> Handle(Query message)
            {
                var availablePermissions = await this.db.Permissions
                    .ProjectTo<PermissionViewModel>()
                    .ToListAsync();

                return new RoleCreateViewModel
                {
                    AvailablePermissions = availablePermissions
                };
            }
        }

        public class Command : IAsyncRequest
        {
            public string Name { get; set; }

            /// <summary>
            /// Holds permission id's
            /// </summary>
            public ICollection<int> SelectedPermissions { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(IRolesValidator validator)
            {
                RuleFor(c => c.Name).MustAsync(validator.RoleNameNotTakenAsync)
                    .WithMessage(ErrorMessages.ROLE_NAME_TAKEN);
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
                UserRole roleToCreate = Mapper.Map<UserRole>(message);
                this.db.Roles.Add(roleToCreate);
                // Called now so the role has an Id when updating the permissions
                await this.db.SaveChangesAsync();
                await UpdatePermissions(roleToCreate, message.SelectedPermissions);
            }

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
                            PermissionRole relationship = new PermissionRole { RoleId = role.Id, PermissionId = permissionInDb.Id };
                            this.db.PermissionsRoles.Add(relationship);
                        }
                    }
                }
            }
        }
    }
}
