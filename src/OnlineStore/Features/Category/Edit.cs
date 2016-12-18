namespace OnlineStore.Features.Category
{
    using AutoMapper;
    using Data;
    using FluentValidation;
    using Infrastructure.Constants;
    using Infrastructure.ViewModels.Categories;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using Util;

    public class Edit
    {
        public class Query : IAsyncRequest<CategoryEditViewModel>
        {
            public int CategoryId { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.CategoryId).NotEqual(0);
            }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, CategoryEditViewModel>
        {
            public QueryHandler(ApplicationDbContext db)
            {
                this.db = db;
            }

            private readonly ApplicationDbContext db;

            public async Task<CategoryEditViewModel> Handle(Query message)
            {
                var categoryInDb = await this.db.Categories
                    .FirstOrDefaultAsync(c => c.Id == message.CategoryId);

                return new CategoryEditViewModel
                {
                    Id = categoryInDb.Id,
                    Name = categoryInDb.Name
                };
            }
        }

        public class Command : IAsyncRequest
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(ICategoryValidator validator)
            {
                RuleFor(c => c.Name).MustAsync(validator.CategoryDoesntExistAsync)
                    .WithMessage(ErrorMessages.CATEGORY_NAME_ALREADY_TAKEN);
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

                Mapper.Map(message, categoryInDb);
                this.db.Entry(categoryInDb).State = EntityState.Modified;
            }
        }

    }
}
