namespace OnlineStore.Features.Permissions
{
    using Data;
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class Remove
    {
        public class Command : IAsyncRequest
        {
            public int? PermissionId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.PermissionId).NotNull();
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
                    .FirstOrDefaultAsync(p => p.Id == message.PermissionId);

                if (permissionInDb != null)
                {
                    this.db.Permissions.Remove(permissionInDb);
                }
            }
        }
    }
}
