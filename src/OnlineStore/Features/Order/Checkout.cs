namespace OnlineStore.Features.Order
{
    using AutoMapper;
    using Data;
    using FluentValidation;
    using Infrastructure.Constants;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Util;

    public class Checkout
    {
        public class Query : IRequest<Command>
        {
            public string SenderUserName { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Command>
        {
            public Command Handle(Query message)
            {
                return new Command
                {
                    SenderUserName = message.SenderUserName
                };
            }
        }

        public class Command : IAsyncRequest
        {
            public string Name { get; set; }
            
            public string Line1 { get; set; }

            public string Line2 { get; set; }

            public string Line3 { get; set; }
            
            public string City { get; set; }
            
            public string State { get; set; }

            public string Zip { get; set; }
            
            public string Country { get; set; }

            public bool GiftWrap { get; set; }

            public string SenderUserName { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(IOrderValidator validator)
            {
                RuleFor(c => c.Name).NotNull().NotEmpty();
                RuleFor(c => c.Line1).NotNull().NotEmpty();
                RuleFor(c => c.City).NotNull().NotEmpty();
                RuleFor(c => c.State).NotNull().NotEmpty();
                RuleFor(c => c.Country).NotNull().NotEmpty();
                RuleFor(c => c.SenderUserName)
                    .MustAsync(validator.UserHasItemsInCartAsync)
                    .WithMessage(ErrorMessages.CART_IS_EMPTY);
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
                var currentUserCart = await this.db.Carts
                    .Include(c => c.OrderedItems)
                        .ThenInclude(oi => oi.Product)
                    .FirstOrDefaultAsync(c => c.User.UserName == message.SenderUserName);

                if (currentUserCart.OrderedItems.Any())
                {
                    var order = new Data.Models.Order();
                    order.DateMade = DateTime.Now;
                    order.OrderedItems = currentUserCart.OrderedItems;
                    Mapper.Map(message, order);
                    this.db.Orders.Add(order);
                }
            }
        }
    }
}
