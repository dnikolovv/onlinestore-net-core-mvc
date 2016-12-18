namespace OnlineStore.Features.Order.Util
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IOrderValidator
    {
        Task<bool> UserHasItemsInCartAsync(string userName, CancellationToken cancellationToken);
    }
}
