namespace OnlineStore.Features.Category.Util
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface ICategoryValidator
    {
        Task<bool> CategoryDoesntExistAsync(string categoryName, CancellationToken cancToken);
    }
}
