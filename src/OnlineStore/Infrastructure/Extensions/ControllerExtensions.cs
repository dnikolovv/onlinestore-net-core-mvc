namespace OnlineStore.Features
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using System.Net;

    public static class ControllerExtensions
    {
        public static ActionResult RedirectToUrlJson<TController>(this TController controller, string url)
            where TController : Controller
        {
            return controller.JsonNet(new { redirect = url });
        }

        public static ActionResult RedirectToActionJson<TController>(this TController controller, string action)
            where TController : Controller
        {
            return controller.JsonNet(new { redirect = controller.Url?.Action(action) });
        }

        public static ActionResult RedirectToActionJson<TController>(this TController controller, string action, string controllerName)
            where TController : Controller
        {
            return controller.JsonNet(new { redirect = controller.Url?.Action(action, controllerName) });
        }

        public static ContentResult JsonNet(this Controller controller, object model, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var serialized = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return new ContentResult
            {
                Content = serialized,
                ContentType = "application/json",
                StatusCode = (int)statusCode
            };
        }
    }
}