namespace OnlineStore.Features.Admin
{
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OnlineStore.Infrastructure.Constants;

    public class AdminController : Controller
    {
        public AdminController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        private readonly IMediator mediator;

        [Authorize(Policy = Policies.PRODUCT_MANAGER)]
        public async Task<ViewResult> Products(Products.Query query)
        {
            var model = await this.mediator.SendAsync(query);
            return View(model);
        }

        [Authorize(Policy = Policies.ORDER_MANAGER)]
        public async Task<ViewResult> Orders(Orders.Query query)
        {
            var model = await this.mediator.SendAsync(query);
            return View(model);
        }

        [Authorize(Policy = Policies.CATEGORY_MANGER)]
        public async Task<ViewResult> Categories(Categories.Query query)
        {
            var model = await this.mediator.SendAsync(query);
            return View(model);
        }

        [Authorize(Policy = Policies.USER_MANAGER)]
        public async Task<ViewResult> Users(Users.Query query)
        {
            var model = await this.mediator.SendAsync(query);
            return View(model);
        }

        [Authorize(Policy = Policies.ROLE_MANAGER)]
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
