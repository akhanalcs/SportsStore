using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        // This dependency injection is handled by ASP.NET core. When it needs to create an instance of this ViewComponent class, it will know
        // that it needs t provide a value for this IStoreRepository parameter and inspect the configuration in Startup class.
        private IStoreRepository repository;
        public NavigationMenuViewComponent(IStoreRepository repo) // This dependency injection is handled by ASP.NET core.
        {
            repository = repo;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(repository.Products.Select(x => x.Category).Distinct().OrderBy(x => x));
        }
    }
}
