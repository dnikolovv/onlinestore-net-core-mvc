namespace OnlineStore.Features.Permissions
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using FluentValidation;
    using Infrastructure.ViewModels.Permissions;
    using Infrastructure.ViewModels.Users;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System;
    using AutoMapper;
    using Data.Models;

    public class Edit
    {
        public class Query : IAsyncRequest<PermissionEditViewModel>
        {
            public int? PermissionId { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.PermissionId).NotNull();
            }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, PermissionEditViewModel>
        {
            public QueryHandler(ApplicationDbContext db)
            {
                this.db = db;
            }

            private readonly ApplicationDbContext db;

            public async Task<PermissionEditViewModel> Handle(Query message)
            {
                var permissionInDb = await this.db.Permissions
                    .Include(p => p.PermissionsRoles)
                        .ThenInclude(pr => pr.Role)
                    .FirstOrDefaultAsync(p => p.Id == message.PermissionId);

                var availableRoles = await this.db.Roles
                    .ProjectTo<RoleViewModel>()
                    .ToListAsync();

                return new PermissionEditViewModel
                {
                    Id = permissionInDb.Id,
                    Action = permissionInDb.Action,
                    Controller = permissionInDb.Controller,
                    AvailableRoles = availableRoles,
                    SelectedRoles = permissionInDb.PermissionsRoles.Select(pr => pr.RoleId).ToList()
                };
            }
        }

        public class Command : IAsyncRequest
        {
            public int? Id { get; set; }

            public string Action { get; set; }

            public string Controller { get; set; }

            public ICollection<int> SelectedRoles { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Id).NotNull();
                RuleFor(c => c.Action).NotNull().NotEmpty();
                RuleFor(c => c.Controller).NotNull().NotEmpty();
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
                var permissionInDb = await this.db.Permissions
                    .Include(p => p.PermissionsRoles)
                    .FirstOrDefaultAsync(p => p.Id == message.Id);

                Mapper.Map(message, permissionInDb);

                await UpdateRoles(permissionInDb, message.SelectedRoles);
            }

            private async Task UpdateRoles(Permission permission, ICollection<int> selectedRoles)
            {
                if (selectedRoles != null && selectedRoles.Count > 0)
                {
                    foreach (var roleId in selectedRoles)
                    {
                        var roleInDb = await this.db.Roles
                            .FirstOrDefaultAsync(r => r.Id == roleId);

                        if (roleInDb != null)
                        {
                            bool relationshipAlreadyExists = permission.PermissionsRoles?
                                .FirstOrDefault(pr => pr.RoleId == roleId && pr.PermissionId == permission.Id) != null;

                            if (!relationshipAlreadyExists)
                            {
                                PermissionRole relationship = new PermissionRole { RoleId = roleId, PermissionId = permission.Id };
                                this.db.PermissionsRoles.Add(relationship);
                            }
                        }
                    }
                }
                else
                {
                    permission.PermissionsRoles?.Clear();
                }
            }
        }
    }
}
