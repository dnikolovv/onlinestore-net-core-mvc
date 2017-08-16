namespace OnlineStore.IntegrationTests
{
    using Microsoft.AspNetCore.Mvc;
    using OnlineStore.Infrastructure.Attributes;

    internal class TestController : Controller
    {
        [ServiceFilter(typeof(DynamicallyAuthorizeServiceFilter))]
        public OkResult TestAction()
        {
            return Ok();
        }
    }
}
