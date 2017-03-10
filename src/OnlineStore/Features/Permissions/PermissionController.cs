namespace OnlineStore.Features.Permissions
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class PermissionController : Controller
    {
        public PermissionController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        private readonly IMediator mediator;
    }
}
