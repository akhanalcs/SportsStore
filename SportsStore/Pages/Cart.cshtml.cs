using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsStore.Infrastructure;
using SportsStore.Models;

namespace SportsStore.Pages
{
    public class CartModel : PageModel
    {
        private IStoreRepository repository;

        //This Cart has access to SessionCart methods and also the Session property.
        public CartModel(IStoreRepository repo, Cart cartService)
        {
            repository = repo;
            Cart = cartService;
        }

        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }

        //Loads the cart
        //After OnPost method this class is intialized, so we need to set ReturnUrl and Cart properties.
        //After this state update task by updating the properties to be used in the View, FINALLY the razor section is rendered in Cart.cshtml
        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
            //Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
        }

        //Updates the cart
        //These 2 parameters come here from ProductSummary page's POST request
        //<form id="@Model.ProductID" asp-page="/Cart" method="post">
        //   <input type="hidden" asp-for="ProductID"/>
        //   <input type = "hidden" name="returnUrl" value="@ViewContext.HttpContext.Request.PathAndQuery()"/>
        public IActionResult OnPost(long productId, string returnUrl)
        {
            var product = repository.Products.FirstOrDefault(p => p.ProductID == productId);
            //Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
            Cart.AddItem(product, 1);
            //HttpContext.Session.SetJson("cart", Cart);
            return RedirectToPage(new { returnUrl = returnUrl }); // It will do this using the Get request. The above OnGet method takes care of it.
        }

        public IActionResult OnPostRemove(long productId, string returnUrl)
        {
            Cart.RemoveLine(Cart.Lines.First(cl => cl.Product.ProductID == productId).Product);
            return RedirectToPage(new { returnUrl = returnUrl });
        }
    }
}
