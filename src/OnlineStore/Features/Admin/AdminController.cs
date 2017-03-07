namespace OnlineStore.Features.Admin
{
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Authorize(Roles = Infrastructure.Constants.Roles.ADMIN_ROLE)]
    public class AdminController : Controller
    {
        public AdminController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        private readonly IMediator mediator;

        public async Task<ViewResult> Products(Products.Query query)
        {
            var model = await this.mediator.SendAsync(query);
            return View(model);
        }

        public async Task<ViewResult> Orders(Orders.Query query)
        {
            var model = await this.mediator.SendAsync(query);
            return View(model);
        }

        public async Task<ViewResult> Categories(Categories.Query query)
        {
            var model = await this.mediator.SendAsync(query);
            return View(model);
        }

        public async Task<ViewResult> Users(Users.Query query)
        {
            var model = await this.mediator.SendAsync(query);
            return View(model);
        }

        public async Task<ViewResult> Roles(Roles.Query query)
        {
            var model = await this.mediator.SendAsync(query);
            return View(model);
        }

        public string GetContentUrl(string givenUrl)
        {
            // Used for calling Url.Content through jquery
            return Url.Content(givenUrl);
        }
    }
}
