namespace OnlineStore.Features.Account
{
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class AccountController : Controller
    {
        public AccountController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        private readonly IMediator mediator;
        
        public ViewResult Login(Login.Query query)
        {
            var model = this.mediator.Send(query);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login.Command command)
        {
            await this.mediator.SendAsync(command);
            return this.RedirectToUrlJson(command.ReturnUrl ?? "/");
        }
        
        public ViewResult Register(Register.Query query)
        {
            var model = this.mediator.Send(query);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register.Command command)
        {
            await this.mediator.SendAsync(command);
            return this.RedirectToActionJson(nameof(Login));
        }

        [Authorize]
        public async Task<RedirectResult> Logout(Logout.Command command)
        {
            await this.mediator.SendAsync(command);
            return Redirect(command.ReturnUrl ?? "/");
        }
    }
}
