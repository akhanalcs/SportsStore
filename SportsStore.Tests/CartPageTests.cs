using System;
using Xunit;
using Moq;
using SportsStore.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.Json;
using SportsStore.Pages;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace SportsStore.Tests
{
    public class CartPageTests
    {
        [Fact]
        public void Can_Load_Cart()
        {
            //Arrange
            var p1 = new Product { ProductID = 1, Name = "P1" };
            var p2 = new Product { ProductID = 2, Name = "P2" };

            var mockRepo = new Mock<IStoreRepository>();
            mockRepo.Setup(m => m.Products).Returns((new Product[] { p1, p2 }).AsQueryable());

            var cart = new Cart();
            cart.AddItem(p1, 2);
            cart.AddItem(p2, 1);

            //Act
            var cartModel = new CartModel(mockRepo.Object, cart);
            cartModel.OnGet("myUrl");

            //Assert
            Assert.Equal(2, cartModel.Cart.Lines.Count);
            Assert.Equal("myUrl", cartModel.ReturnUrl);
        }

        [Fact]
        public void Can_Update_Cart()
        {
            //Arrange
            var mockRepo = new Mock<IStoreRepository>();
            mockRepo.Setup(m => m.Products).Returns((new Product[]
            {
                new Product{ProductID = 1, Name = "P1"}
            }).AsQueryable());

            var testCart = new Cart();

            //Act
            var cartModel = new CartModel(mockRepo.Object, testCart);

            cartModel.OnPost(1, "myUrl");

            //Assert
            Assert.Single(testCart.Lines);
            Assert.Equal("P1", testCart.Lines.First().Product.Name);
            Assert.Equal(1, testCart.Lines.First().Quantity);
        }
    }
}
