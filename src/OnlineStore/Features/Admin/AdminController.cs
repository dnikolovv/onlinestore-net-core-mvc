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

        [Authorize(Policy = Policies.PRODUCTS_POLICY)]
        public async Task<ViewResult> Products(Products.Query query)
        {
            var model = await this.mediator.SendAsync(query);
            return View(model);
        }

        [Authorize(Policy = Policies.ORDERS_POLICY)]
        public async Task<ViewResult> Orders(Orders.Query query)
        {
            var model = await this.mediator.SendAsync(query);
            return View(model);
        }

        [Authorize(Policy = Policies.CATEGORIES_POLICY)]
        public async Task<ViewResult> Categories(Categories.Query query)
        {
            var model = await this.mediator.SendAsync(query);
            return View(model);
        }

        [Authorize(Policy = Policies.USERS_POLICY)]
        public async Task<ViewResult> Users(Users.Query query)
        {
            var model = await this.mediator.SendAsync(query);
            return View(model);
        }

        [Authorize(Policy = Policies.ROLES_POLICY)]
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
