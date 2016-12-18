namespace OnlineStore.Features.Category
{
    using Data;
    using FluentValidation;
    using Infrastructure.Constants;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class Remove
    {
        public class Query : IAsyncRequest<Command>
        {
            public int CategoryId { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(c => c.CategoryId).NotEqual(0);
            }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, Command>
        {
            public QueryHandler(ApplicationDbContext db)
            {
                this.db = db;
            }

            private readonly ApplicationDbContext db;

            public async Task<Command> Handle(Query message)
            {
                var categoryInDb = await this.db.Categories
                    .FirstOrDefaultAsync(c => c.Id == message.CategoryId);

                var countOfProductsInThisCategory = await this.db.Products
                    .Where(p => p.Category.Id == message.CategoryId)
                    .CountAsync();

                return new Command
                {
                    Id = message.CategoryId,
                    Name = categoryInDb?.Name,
                    ProductsInCategoryCount = countOfProductsInThisCategory
                };
            }
        }

        public class Command : IAsyncRequest
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public int ProductsInCategoryCount { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Id).NotEqual(0)
                    .WithMessage(ErrorMessages.CATEGORY_DOESNT_EXIST);
                RuleFor(c => c.Name).NotNull().NotEmpty();
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
                var categoryInDb = await this.db.Categories
                    .FirstOrDefaultAsync(c => c.Id == message.Id);

                var productsInCategory = await this.db.Products
                    .Where(p => p.Category.Id == categoryInDb.Id)
                    .ToListAsync();

                this.db.Products.RemoveRange(productsInCategory);
                this.db.Categories.Remove(categoryInDb);
            }
        }
    }
}
