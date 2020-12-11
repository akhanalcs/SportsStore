using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private Cart cart;

        public OrderController(IOrderRepository repoService, Cart cartService)
        {
            repository = repoService;
            cart = cartService;
        }

        public ViewResult Checkout() // This action will just render View with an empty object.
        {
            return View(new Order());
        }

        //This will handle POST requests
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if(cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty.");
            }
            if (ModelState.IsValid) // Make sure the Model bound values are correct such as in typing and so on.
            {
                order.Lines = cart.Lines.ToArray();
                repository.SaveOrder(order);
                cart.Clear();
                return RedirectToPage("/Completed", new { orderId = order.OrderID });
            }
            else
            {
                return View(); //Checkout.cshtml
            }
        }
    }
}