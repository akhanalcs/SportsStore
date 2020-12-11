using System;
using Xunit;
using Moq;
using SportsStore.Models;
using System.Linq;

namespace SportsStore.Tests
{
    public class CartTests
    {
        [Fact]
        public void Can_Add_New_Lines()
        {
            //Arrange
            var p1 = new Product { ProductID = 1, Name = "P1" };
            var p2 = new Product { ProductID = 2, Name = "P2" };

            var cart = new Cart();

            //Act
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            var lines = cart.Lines.ToArray();

            //Assert
            Assert.Equal(2, lines.Length);
            Assert.Equal(p1, lines[0].Product);
            Assert.Equal(p2, lines[1].Product);
        }

        [Fact]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            //Arrange
            var p1 = new Product { ProductID = 1, Name = "P1" };
            var p2 = new Product { ProductID = 2, Name = "P2" };

            var cart = new Cart();
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.AddItem(p1, 10);

            //Act
            var lines = cart.Lines.OrderBy(c => c.Product.ProductID).ToArray();

            //Assert
            Assert.Equal(2, lines.Length);
            Assert.Equal(11, lines[0].Quantity);
            Assert.Equal(1, lines[1].Quantity);
        }

        [Fact]
        public void Can_Remove_Line()
        {
            //Arrange
            var p1 = new Product { ProductID = 1, Name = "P1" };
            var p2 = new Product { ProductID = 2, Name = "P2" };
            var p3 = new Product { ProductID = 3, Name = "P3" };

            var cart = new Cart();
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 3);
            cart.AddItem(p3, 5);
            cart.AddItem(p2, 1);

            //Act
            cart.RemoveLine(p2);

            //Assert
            Assert.Empty(cart.Lines.Where(c => c.Product == p2));
            Assert.Equal(2, cart.Lines.Count);
        }

        [Fact]
        public void Calculate_Cart_Total()
        {
            //Arrange
            var p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            var p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

            var cart = new Cart();

            //Act
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.AddItem(p1, 3);

            //Assert
            Assert.Equal(450M, cart.ComputeTotalValue());
        }

        [Fact]
        public void Can_Clear_Contents()
        {
            //Arrange
            var p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            var p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

            var cart = new Cart();

            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);

            //Act
            cart.Clear();

            //Assert
            Assert.Empty(cart.Lines);
        }
    }
}
