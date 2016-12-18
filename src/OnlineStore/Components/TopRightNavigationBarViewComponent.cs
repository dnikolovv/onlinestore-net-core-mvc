namespace OnlineStore.Components
{
    using Microsoft.AspNetCore.Mvc;

    public class TopNavigationBarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
