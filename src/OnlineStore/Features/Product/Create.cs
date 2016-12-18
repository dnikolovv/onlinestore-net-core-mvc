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
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class Create
    {
        public class Query : IAsyncRequest<ProductEditViewModel> { }

        public class QueryHandler : IAsyncRequestHandler<Query, ProductEditViewModel>
        {
            public QueryHandler(ApplicationDbContext db)
            {
                this.db = db;
            }

            private readonly ApplicationDbContext db;

            public async Task<ProductEditViewModel> Handle(Query message)
            {
                return new ProductEditViewModel
                {
                    Categories = new SelectList(await this.db.Categories
                        .Select(c => c.Name)
                        .ToListAsync())
                };
            }
        }

        public class Command : IAsyncRequest
        {
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
                { // Add it to the db
                    this.db.Categories.Add(message.Category);
                }

                // and map the command to a product
                Product product = new Product();
                Mapper.Map(message, product);
                product.DateAdded = DateTime.Now;
                this.db.Products.Add(product);
            }
        }
    }
}
