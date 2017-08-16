namespace OnlineStore.Features.Roles
{
    using Infrastructure.Attributes;
    using Infrastructure.Constants;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [ServiceFilter(typeof(DynamicallyAuthorizeServiceFilter))]
    public class RoleController : Controller
    {
        public RoleController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        private readonly IMediator mediator;

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
            TempData["successMessage"] = SuccessMessages.SuccessfullyCreatedRole(command.Name);
            return this.RedirectToActionJson("Roles", "Admin");
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
            TempData["successMessage"] = SuccessMessages.SuccessfullyEditedRole(command.Name);
            return this.RedirectToActionJson("Roles", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(Remove.Command command)
        {
            await this.mediator.SendAsync(command);
            TempData["successMessage"] = SuccessMessages.SuccessfullyDeletedRole((int)command.RoleId);
            return this.RedirectToActionJson("Roles", "Admin");
        }
    }
}
