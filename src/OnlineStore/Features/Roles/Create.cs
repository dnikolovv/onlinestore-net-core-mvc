namespace OnlineStore.Features.Roles
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using FluentValidation;
    using Infrastructure.Constants;
    using Infrastructure.ViewModels.Roles;
    using MediatR;
    using OnlineStore.Infrastructure.Authorization;
    using Util;

    public class Create
    {
        public class Query : IRequest<RoleCreateViewModel> { }

        public class QueryHandler : IRequestHandler<Query, RoleCreateViewModel>
        {
            public QueryHandler(ApplicationDbContext db)
            {
                this.db = db;
            }

            private readonly ApplicationDbContext db;

            public RoleCreateViewModel Handle(Query message)
            {
                return new RoleCreateViewModel
                {
                    AvailableSections = AuthorizationSectionsContainer
                        .Sections
                        .Select(section => new AuthorizationSectionViewModel
                        {
                            ClaimType = section.ClaimType,
                            FriendlyName = section.FriendlyName,
                            Role = null
                        })
                        .ToList()
                };
            }
        }

        public class Command : IRequest
        {
            public string Name { get; set; }

            public ICollection<int> SelectedClaims { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(IRolesValidator validator)
            {
                RuleFor(c => c.Name).MustAsync(validator.RoleNameNotTakenAsync)
                    .WithMessage(ErrorMessages.ROLE_NAME_TAKEN);
            }
        }

        public class CommandHandler : RequestHandler<Command>
        {
            public CommandHandler(ApplicationDbContext db)
            {
                this.db = db;
            }

            private readonly ApplicationDbContext db;
            
            protected override void HandleCore(Command message)
            {
                UserRole roleToCreate = Mapper.Map<UserRole>(message);
                this.db.Roles.Add(roleToCreate);
            }
        }
    }
}
