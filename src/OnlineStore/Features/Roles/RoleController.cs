namespace OnlineStore.Features.Roles
{
    using Infrastructure.Attributes;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [ServiceFilter(typeof(DynamicallyAuthorize))]
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
            return this.RedirectToActionJson("Roles", "Admin");
        }
    }
}
