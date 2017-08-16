namespace OnlineStore.Features.Product
{
    using Features;
    using Infrastructure.Attributes;
    using Infrastructure.Constants;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class ProductController : Controller
    {
        public ProductController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        private readonly IMediator mediator;
        
        public async Task<ViewResult> Details(Details.Query query)
        {
            var model = await this.mediator.SendAsync(query);
            return View(model);
        }

        public async Task<ViewResult> ListLatest(ListLatest.Query query)
        {
            var model = await this.mediator.SendAsync(query);
            return View(model);
        }

        public async Task<ViewResult> List(List.Query query)
        {
            var model = await this.mediator.SendAsync(query);
            return View(model);
        }

        [ServiceFilter(typeof(DynamicallyAuthorizeServiceFilter))]
        [HttpGet]
        public async Task<IActionResult> Edit(Edit.Query query)
        {
            var model = await this.mediator.SendAsync(query);
            return View(model);
        }

        [ServiceFilter(typeof(DynamicallyAuthorizeServiceFilter))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Edit.Command command)
        {
            await this.mediator.SendAsync(command);
            TempData["successMessage"] = SuccessMessages.SuccessfullyEditedProduct(command.Name);
            return this.RedirectToActionJson("Products", "Admin");
        }

        [ServiceFilter(typeof(DynamicallyAuthorizeServiceFilter))]
        [HttpGet]
        public async Task<ViewResult> Create(Create.Query query)
        {
            var model = await this.mediator.SendAsync(query);
            return View(model);
        }

        [ServiceFilter(typeof(DynamicallyAuthorizeServiceFilter))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Create.Command command)
        {
            await this.mediator.SendAsync(command);
            TempData["successMessage"] = SuccessMessages.SuccessfullyCreatedProduct(command.Name);
            return this.RedirectToActionJson("Products", "Admin");
        }

        [ServiceFilter(typeof(DynamicallyAuthorizeServiceFilter))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(Remove.Command command)
        {
            await this.mediator.SendAsync(command);
            TempData["successMessage"] = SuccessMessages.SuccessfullyRemovedProduct(command.ProductId);
            return RedirectToAction("Products", "Admin");
        }
    }
}
