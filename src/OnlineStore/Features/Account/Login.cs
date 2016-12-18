namespace OnlineStore.Features.Account
{
    using FluentValidation;
    using Infrastructure.Constants;
    using Infrastructure.Services.Contracts;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;
    using Util;

    public class Login
    {
        public class Query : IRequest<Command>
        {
            public string ReturnUrl { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Command>
        {
            public Command Handle(Query message)
            {
                return new Command
                {
                    ReturnUrl = message.ReturnUrl
                };
            }
        }

        public class Command : IAsyncRequest<bool>
        {
            public string UserName { get; set; }

            public string Password { get; set; }

            public string ReturnUrl { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(IUserValidator validator)
            {
                RuleFor(c => c.UserName).NotNull().NotEmpty();
                RuleFor(c => c.Password).NotNull().NotEmpty();

                RuleFor(c => c.UserName)
                    .MustAsync(async (command, userName, cancToken) => 
                        await validator.ValidUserAndPasswordAsync(command.Password, userName, CancellationToken.None))
                    .WithMessage(ErrorMessages.INVALID_USERNAME_OR_PASSWORD);
            }
        }

        public class CommandHandler : IAsyncRequestHandler<Command, bool>
        {
            public CommandHandler(IUsersService usersService)
            {
                this.usersService = usersService;
            }

            private readonly IUsersService usersService;

            public async Task<bool> Handle(Command message)
            {
                if (await this.usersService.LoginAsync(message.UserName, message.Password))
                {
                    return true;
                }

                return false;
            }
        }
    }
}
