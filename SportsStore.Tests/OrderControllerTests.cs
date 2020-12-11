using System;
using Xunit;
using Moq;
using SportsStore.Models;
using SportsStore.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace SportsStore.Tests
{
    public class OrderControllerTests
    {
        [Fact]
        public void Cannot_Checkout_Empty_Cart()
        {
            //Arrange
            var mock = new Mock<IOrderRepository>();
            var cart = new Cart();
            var order = new Order();
            //Act
            var target = new OrderController(mock.Object, cart);
            var result = target.Checkout(order) as ViewResult;

            //Assert
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never); //ensuring that the SaveOrder of the mock IOrderRepository implementation is never called
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            //Arrange
            var mock = new Mock<IOrderRepository>();
            var cart = new Cart();
            var product = new Product();
            cart.AddItem(product, 1);

            var order = new Order();
            var target = new OrderController(mock.Object, cart);
            target.ModelState.AddModelError("", "Some error");

            //Act
            var result = target.Checkout(order) as ViewResult;
            //Assert
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never); //ensuring that the SaveOrder of the mock IOrderRepository implementation is never called
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Can_Checkout_And_Submit_Order()
        {
            //Arrange
            var mock = new Mock<IOrderRepository>();
            var cart = new Cart();
            var product = new Product();
            cart.AddItem(product, 1);
            var order = new Order();

            //Act
            var target = new OrderController(mock.Object, cart);
            var result = target.Checkout(order) as RedirectToPageResult;
            //Assert
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
            Assert.Equal("/Completed", result.PageName);
        }
    }
}
