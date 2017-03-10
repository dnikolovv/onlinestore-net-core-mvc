namespace OnlineStore.Features.Permissions
{
    using Infrastructure.Constants;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class PermissionController : Controller
    {
        public PermissionController(IMediator mediator)
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
        public IActionResult Create(Create.Command command)
        {
            this.mediator.Send(command);
            TempData["successMessage"] = SuccessMessages.SuccessfullyCreatedPermission();
            return this.RedirectToActionJson("Permissions", "Admin");
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
            TempData["successMessage"] = SuccessMessages.SuccessfullyEditedPermission();
            return this.RedirectToActionJson("Permissions", "Admin");
        }
    }
}
