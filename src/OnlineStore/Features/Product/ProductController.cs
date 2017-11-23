namespace OnlineStore.Features.Product
{
    using System.Threading.Tasks;
    using Features;
    using Infrastructure.Constants;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OnlineStore.Infrastructure.Extensions;

    [Authorize(Policy = Policies.PRODUCTS_POLICY)]
    public class ProductController : Controller
    {
        public ProductController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        private readonly IMediator mediator;
        
        [AllowAnonymous]
        public async Task<ViewResult> Details(Details.Query query)
        {
            var model = await this.mediator.SendAsync(query);
            return View(model);
        }

        [AllowAnonymous]
        public async Task<ViewResult> ListLatest(ListLatest.Query query)
        {
            var model = await this.mediator.SendAsync(query);
            return View(model);
        }

        [AllowAnonymous]
        public async Task<ViewResult> List(List.Query query)
        {
            var model = await this.mediator.SendAsync(query);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Edit.Query query)
        {
            var model = await this.mediator.SendAsync(query);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Edit.Command command)
        {
            await this.mediator.SendAsync(command);
            TempData.SetSuccessMessage(SuccessMessages.SuccessfullyEditedProduct(command.Name));
            return this.RedirectToActionJson("Products", "Admin");
        }
        
        [HttpGet]
        public async Task<ViewResult> Create(Create.Query query)
        {
            var model = await this.mediator.SendAsync(query);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Create.Command command)
        {
            await this.mediator.SendAsync(command);
            TempData.SetSuccessMessage(SuccessMessages.SuccessfullyCreatedProduct(command.Name));
            return this.RedirectToActionJson("Products", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(Remove.Command command)
        {
            await this.mediator.SendAsync(command);
            TempData.SetSuccessMessage(SuccessMessages.SuccessfullyRemovedProduct(command.ProductId));
            return RedirectToAction("Products", "Admin");
        }
    }
}
