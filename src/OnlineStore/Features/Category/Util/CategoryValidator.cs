namespace OnlineStore.Features.Category.Util
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using System.Threading;
    using System.Threading.Tasks;

    public class CategoryValidator : ICategoryValidator
    {
        public CategoryValidator(ApplicationDbContext db)
        {
            this.db = db;
        }

        private readonly ApplicationDbContext db;

        public async Task<bool> CategoryDoesntExistAsync(string categoryName, CancellationToken cancToken)
        {
            var categoryInDb = await this.db.Categories
                .FirstOrDefaultAsync(c => c.Name == categoryName);

            return categoryInDb == null;
        }
    }
}
