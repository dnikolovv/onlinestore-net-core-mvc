namespace OnlineStore.Features.Category
{
    using AutoMapper;
    using Data;
    using FluentValidation;
    using Infrastructure.Constants;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using Util;

    public class Create
    {
        public class Command : IAsyncRequest
        {
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
                    .FirstOrDefaultAsync(c => c.Name == message.Name);

                if (categoryInDb == null)
                {
                    var category = new Data.Models.Category();
                    Mapper.Map(message, category);
                    this.db.Categories.Add(category);
                }
            }
        }
    }
}
