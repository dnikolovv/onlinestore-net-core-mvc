namespace OnlineStore.Features.Roles
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
            public int? RoleId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.RoleId).NotNull();
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
                    .FirstOrDefaultAsync(r => r.Id == message.RoleId);

                if (roleInDb != null)
                {
                    this.db.Roles.Remove(roleInDb);
                }
            }
        }
    }
}
