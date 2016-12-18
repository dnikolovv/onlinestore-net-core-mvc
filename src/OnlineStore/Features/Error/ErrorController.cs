namespace OnlineStore.Features.Error
{
    using Microsoft.AspNetCore.Mvc;

    public class ErrorController : Controller
    {
        public ViewResult Error() => View();
    }
}
