namespace OnlineStore.Features.Category
{
    using System.Threading.Tasks;
    using Infrastructure.Constants;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OnlineStore.Infrastructure.Extensions;

    [Authorize(Policy = Policies.CATEGORY_MANGER)]
    public class CategoryController : Controller
    {
        public CategoryController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        private readonly IMediator mediator;

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Create.Command command)
        {
            await this.mediator.SendAsync(command);
            TempData.SetSuccessMessage(SuccessMessages.SuccessfullyCreatedCategory(command.Name));
            return this.RedirectToActionJson("Categories", "Admin");
        }

        [HttpGet]
        public async Task<ViewResult> Edit(Edit.Query query)
        {
            var model = await this.mediator.SendAsync(query);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Edit.Command command)
        {
            await this.mediator.SendAsync(command);
            TempData.SetSuccessMessage(SuccessMessages.SuccessfullyEditedCategory(command.Name));
            return this.RedirectToActionJson("Categories", "Admin");
        }

        [HttpGet]
        public async Task<ViewResult> Remove(Remove.Query query)
        {
            var model = await this.mediator.SendAsync(query);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(Remove.Command command)
        {
            await this.mediator.SendAsync(command);
            TempData.SetSuccessMessage(SuccessMessages.SuccessfullyRemovedCategory(command.Name));
            return this.RedirectToActionJson("Categories", "Admin");
        }
    }
}
