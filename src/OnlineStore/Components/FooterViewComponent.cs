namespace OnlineStore.Components
{
    using Microsoft.AspNetCore.Mvc;

    public class FooterViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
