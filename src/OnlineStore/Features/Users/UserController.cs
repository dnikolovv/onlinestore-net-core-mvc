namespace OnlineStore.Features.Users
{
    using System.Threading.Tasks;
    using Infrastructure.Constants;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OnlineStore.Infrastructure.Extensions;

    [Authorize(Policy = Policies.USER_MANAGER)]
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
            TempData.SetSuccessMessage(SuccessMessages.SuccessfullyEditedUser(command.UserName));
            return this.RedirectToActionJson("Users", "Admin");
        }
    }
}
