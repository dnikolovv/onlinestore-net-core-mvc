namespace OnlineStore.Components
{
    using AutoMapper;
    using Data;
    using Infrastructure.ViewModels.Cart;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class CartSummaryViewComponent : ViewComponent
    {
        public CartSummaryViewComponent(ApplicationDbContext db)
        {
            this.db = db;
        }

        private readonly ApplicationDbContext db;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUserCart = await this.db.Carts
                .Include(c => c.OrderedItems)
                    .ThenInclude(c => c.Product)
                .FirstOrDefaultAsync(c => c.User.UserName == this.User.Identity.Name);

            var viewModel = Mapper.Map<CartViewModel>(currentUserCart);

            return View(viewModel);
        }
    }
}
