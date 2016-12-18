namespace OnlineStore.Features.Account
{
    using Infrastructure.Services.Contracts;
    using MediatR;
    using System.Threading.Tasks;

    public class Logout
    {
        public class Command : IAsyncRequest
        {
            public string ReturnUrl { get; set; }
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
                await this.usersService.LogoutAsync();
            }
        }
    }
}
