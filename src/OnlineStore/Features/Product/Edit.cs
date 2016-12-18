namespace OnlineStore.Features.Product
{
    using AutoMapper;
    using Data;
    using Data.Models;
    using FluentValidation;
    using Infrastructure.Extensions;
    using Infrastructure.ViewModels.Products;
    using MediatR;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class Edit
    {
        public class Query : IAsyncRequest<ProductEditViewModel>
        {
            public int ProductId { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.ProductId).NotEqual(0);
            }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, ProductEditViewModel>
        {
            public QueryHandler(ApplicationDbContext db)
            {
                this.db = db;
            }

            private readonly ApplicationDbContext db;

            public async Task<ProductEditViewModel> Handle(Query message)
            {
                var product = await this.db.Products
                    .Where(p => p.Id == message.ProductId)
                    .FirstOrDefaultAsync();

                var categories = await this.db.Categories.ToListAsync();

                ProductEditViewModel result = Mapper.Map<ProductEditViewModel>(product);
                result.Categories = new SelectList(categories.Select(c => c.Name), product.Category.Name);

                return result;
            }
        }

        public class Command : IAsyncRequest
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public string ImagePath { get; set; }

            public decimal Price { get; set; }

            public Category Category { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Name).NotNull().NotEmpty();
                RuleFor(c => c.Price).InclusiveBetween(0, decimal.MaxValue);
                RuleFor(c => c.Description).NotEmpty();
                RuleFor(c => c.Category).NotNull();
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
                // Then check if such category exists in the db
                Category categoryInDb = await this.db.Categories.FirstOrDefaultAsync(c => c.Name == message.Category.Name);

                if (categoryInDb != null)
                { // If it's there, set it as the command's category
                    message.Category = categoryInDb;
                }
                else
                { // Else, add it to the db
                    this.db.Categories.Add(message.Category);
                }

                // Get the product with that Id
                var product = await this.db.Products.FirstOrDefaultAsync(p => p.Id == message.Id);
                // Set his values and save
                Mapper.Map(message, product);
            }
        }
    }
}
