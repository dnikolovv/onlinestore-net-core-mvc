namespace OnlineStore.Features.Roles
{
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Infrastructure.Constants;

    [Authorize(Roles = Roles.ADMIN_ROLE)]
    public class RoleController : Controller
    {
        public RoleController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        private readonly IMediator mediator;
    }
}
