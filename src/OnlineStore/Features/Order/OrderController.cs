namespace OnlineStore.Features.Order
{
    using Infrastructure.Attributes;
    using Infrastructure.Constants;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Authorize]
    public class OrderController : Controller
    {
        public OrderController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        private readonly IMediator mediator;

        [HttpPost]
        [ServiceFilter(typeof(DynamicallyAuthorizeServiceFilter))]
        public async Task<IActionResult> MarkShipped(MarkShipped.Command command)
        {
            await this.mediator.SendAsync(command);
            return this.RedirectToActionJson("Orders", "Admin");
        }

        [HttpGet]
        public ViewResult Checkout(Checkout.Query query)
        {
            var model = this.mediator.Send(query);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Checkout.Command command)
        {
            await this.mediator.SendAsync(command);
            return this.RedirectToActionJson(nameof(Completed));
        }
        
        public async Task<ViewResult> Completed(Completed.Command command)
        {
            command.CurrentUserName = this.User.Identity.Name;
            await this.mediator.SendAsync(command);    
            return View();
        }
    }
}
