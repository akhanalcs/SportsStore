using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class HomeController : Controller // Will create a new instance of this class to handle an HTTP request.
        //It will see that it needs an object in the ctor that implements IStoreRepository interface.
        //TO determine which implementation class should be used, it looks at Startup.cs to see that EFStoreRepository should be used.
        //New instance of EFStoreRepository is created for every request
    {
        private IStoreRepository repository;
        public int PageSize = 4;

        public HomeController(IStoreRepository repo) // At every request http://localhost:5000/?productPage=1 or 2 and so on, it'll hit this controller and the Indoex method.
        {
            repository = repo;
        }

        public ViewResult Index(string category, int productPage = 1)
        {
            var products = repository.Products
                                     .Where(p => p.Category == category || category == null)
                                     .OrderBy(p => p.ProductID)
                                     .Skip((productPage - 1) * PageSize)
                                     .Take(PageSize);

            var productListVM = new ProductsListViewModel
            {
                Products = products,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ? repository.Products.Count() : repository.Products.Where(e => e.Category == category).Count()
                },
                CurrentCategory = category
            };

            return View(productListVM);
        }
    }
}