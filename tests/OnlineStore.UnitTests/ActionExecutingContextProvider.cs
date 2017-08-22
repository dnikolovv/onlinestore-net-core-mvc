namespace OnlineStore.UnitTests
{
    using FakeItEasy;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Routing;
    using System.Collections.Generic;

    public static class ActionExecutingContextProvider
    {
        public static ActionExecutingContext GetActionExecutingContext(string requestMethod)
        {
            var httpContext = A.Fake<HttpContext>();
            httpContext.Request.Method = requestMethod;

            var actionContext = new ActionContext()
            {
                HttpContext = httpContext,
                RouteData = A.Fake<RouteData>(),
                ActionDescriptor = A.Fake<ActionDescriptor>()
            };

            var filterContext = A.Fake<ActionExecutingContext>(context => context.WithArgumentsForConstructor(() => new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                A.Fake<Controller>())));

            return filterContext;
        }
    }
}
