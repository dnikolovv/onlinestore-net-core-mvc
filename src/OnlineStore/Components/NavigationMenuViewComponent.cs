namespace OnlineStore.Components
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Infrastructure.ViewModels.Categories;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    public class NavigationMenuViewComponent : ViewComponent
    {
        public NavigationMenuViewComponent(ApplicationDbContext db)
        {
            this.db = db;
        }

        private readonly ApplicationDbContext db;

        public IViewComponentResult Invoke()
        {
            var categories = this.db.Categories
                .ProjectTo<CategoryViewModel>()
                .OrderBy(c => c.Name);

            return View(categories);
        }
    }
}
