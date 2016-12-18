namespace OnlineStore.Features.Account.Util
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IUserValidator
    {
        Task<bool> NameNotTakenAsync(string userName, CancellationToken cancellationToken);

        Task<bool> ValidUserAndPasswordAsync(string password, string userName, CancellationToken cancellationToken);

        bool PasswordMatchesConfirmation(string password, string confirmedPassword);
    }
}
