namespace OnlineStore.IntegrationTests.Features.Roles
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class RolesController : Controller
    {
        public RolesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        private readonly IMediator mediator;
    }
}
