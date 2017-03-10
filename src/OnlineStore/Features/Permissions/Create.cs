namespace OnlineStore.Features.Permissions
{
    using AutoMapper;
    using Data;
    using Data.Models;
    using FluentValidation;
    using Infrastructure.Constants;
    using MediatR;
    using System.Threading;
    using Util;

    public class Create
    {
        public class Command : IRequest
        {
            public string Action { get; set; }

            public string Controller { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(IPermissionValidator validator)
            {
                RuleFor(c => c.Action).MustAsync((command, action, cancToken) =>
                    validator.PermissionDoesntExistAsync(command.Controller, action, CancellationToken.None))
                    .WithMessage(ErrorMessages.PERMISSION_ALREADY_EXISTS);
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
                Permission permissionToCreate = Mapper.Map<Permission>(message);
                this.db.Permissions.Add(permissionToCreate);
            }
        }
    }
}
