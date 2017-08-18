namespace OnlineStore.Features.Account
{
    using AutoMapper;
    using Data.Models;
    using FluentValidation;
    using Infrastructure.Constants;
    using Infrastructure.Services.Contracts;
    using MediatR;
    using OnlineStore.Data;
    using System.Threading.Tasks;
    using Util;

    public class Register
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

        public class Command : IAsyncRequest
        {
            public string UserName { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string Password { get; set; }

            public string ConfirmedPassword { get; set; }

            public string ReturnUrl { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(IUserValidator validator)
            {
                RuleFor(c => c.UserName).NotNull().NotEmpty();
                RuleFor(c => c.UserName)
                    .MustAsync(validator.NameNotTakenAsync).WithMessage(ErrorMessages.USERNAME_ALREADY_TAKEN);

                RuleFor(c => c.FirstName).NotNull().NotEmpty();
                RuleFor(c => c.Password).NotNull().NotEmpty();
                RuleFor(c => c.ConfirmedPassword)
                    .Must((command, confirmedPassword) => validator.PasswordMatchesConfirmation(command.Password, confirmedPassword))
                    .WithMessage(ErrorMessages.PASSWORDS_DONT_MATCH);
            }
        }

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            public CommandHandler(IUsersService usersService)
            {
                this.usersService = usersService;
            }

            private readonly IUsersService usersService;

            protected override async Task HandleCore(Command message)
            {
                var user = new User();
                Mapper.Map(message, user);

                await this.usersService.RegisterAsync(user, message.Password);
            }
        }
    }
}
