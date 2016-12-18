namespace OnlineStore.Features.Product
{
    using AutoMapper;
    using Data;
    using Infrastructure.ViewModels.Products;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class Details
    {
        public class Query : IAsyncRequest<ProductViewModel>
        {
            public int ProductId { get; set; }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, ProductViewModel>
        {
            public QueryHandler(ApplicationDbContext db)
            {
                this.db = db;
            }

            private readonly ApplicationDbContext db;

            public async Task<ProductViewModel> Handle(Query message)
            {
                var productInDb = await this.db
                    .Products
                    .FirstOrDefaultAsync(p => p.Id == message.ProductId);

                var viewModel = new ProductViewModel();
                Mapper.Map(productInDb, viewModel);

                return viewModel;
            }
        }
    }
}
