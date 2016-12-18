namespace OnlineStore.Features.Account.Util
{
    using Infrastructure.Services.Contracts;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class UserValidator : IUserValidator
    {
        public UserValidator(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        private readonly IUsersService usersService;

        public async Task<bool> NameNotTakenAsync(string userName, CancellationToken cancellationToken)
        {
            if (!String.IsNullOrEmpty(userName))
            {
                var userInDb = await this.usersService
                            .FindByUserNameAsync(userName);

                return userInDb == null;
            }

            // Must return true so it doesn't throw the username is already taken error
            return true;
        }

        public async Task<bool> ValidUserAndPasswordAsync(string password, string userName, CancellationToken cancellationToken)
        {
            if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(password))
            {
                var successfulLogin = await this.usersService.LoginAsync(userName, password);

                return successfulLogin;
            }

            // Must return true so it doesn't throw the invalid username or pass error
            return true;
        }

        public bool PasswordMatchesConfirmation(string password, string confirmedPassword)
        {
            return password == confirmedPassword;
        }
    }
}
