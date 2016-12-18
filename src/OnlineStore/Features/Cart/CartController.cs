namespace OnlineStore.Features.Cart
{
    using Infrastructure.Attributes;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [JsonAuthorize]
    public class CartController : Controller
    {
        public CartController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        private readonly IMediator mediator;

        public async Task<ViewResult> Index(Index.Query query)
        {
            query.CurrentUserName = User.Identity.Name;
            var model = await this.mediator.SendAsync(query);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task AddToCart(AddToCart.Command query)
        {
            query.CurrentUserName = User.Identity.Name;
            await this.mediator.SendAsync(query);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCart(RemoveFromCart.Command query)
        {
            query.CurrentUserName = this.User.Identity.Name;
            await this.mediator.SendAsync(query);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult RefreshCartSummary()
        {
            return ViewComponent("CartSummary");
        }
    }
}
