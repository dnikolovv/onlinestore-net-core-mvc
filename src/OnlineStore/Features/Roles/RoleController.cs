namespace OnlineStore.Features.Roles
{
    using System.Threading.Tasks;
    using Infrastructure.Constants;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OnlineStore.Infrastructure.Extensions;

    [Authorize(Policy = Policies.ROLE_MANAGER)]
    public class RoleController : Controller
    {
        public RoleController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        private readonly IMediator mediator;

        [HttpGet]
        public IActionResult Create(Create.Query query)
        {
            var model = this.mediator.Send(query);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Create.Command command)
        {
            this.mediator.Send(command);
            TempData.SetSuccessMessage(SuccessMessages.SuccessfullyCreatedRole(command.Name));
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
            TempData.SetSuccessMessage(SuccessMessages.SuccessfullyEditedRole(command.Name));
            return this.RedirectToActionJson("Roles", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(Remove.Command command)
        {
            await this.mediator.SendAsync(command);
            TempData.SetSuccessMessage(SuccessMessages.SuccessfullyDeletedRole((int)command.RoleId));
            return this.RedirectToActionJson("Roles", "Admin");
        }
    }
}
