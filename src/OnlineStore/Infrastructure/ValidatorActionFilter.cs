namespace OnlineStore.Infrastructure
{
    using System.Net;
    using System.Net.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Newtonsoft.Json;

    public class ValidatorActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.ModelState.IsValid)
            {
                if (filterContext.HttpContext.Request.Method == HttpMethod.Get.Method)
                {
                    filterContext.Result = new BadRequestResult();
                }
                else
                {
                    var result = new ContentResult();

                    string content = JsonConvert.SerializeObject(filterContext.ModelState,
                        new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });

                    result.Content = content;
                    result.ContentType = "application/json";

                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    filterContext.Result = result;
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}