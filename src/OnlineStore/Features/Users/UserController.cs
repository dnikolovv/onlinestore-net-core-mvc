namespace OnlineStore.Features.Users
{
    using Infrastructure.Attributes;
    using Infrastructure.Constants;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [ServiceFilter(typeof(DynamicallyAuthorize))]
    public class UserController : Controller
    {
        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        private readonly IMediator mediator;

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
            TempData["SuccessMessage"] = SuccessMessages.SuccessfullyEditedUser(command.UserName);
            return this.RedirectToActionJson("Users", "Admin");
        }
    }
}
