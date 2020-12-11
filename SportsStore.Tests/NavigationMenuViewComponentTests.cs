using System;
using Moq;
using SportsStore.Models;
using Xunit;
using System.Linq;
using SportsStore.Components;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void Can_Select_Categories()
        {
            //Arrange
            var mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product{ ProductID = 1, Name = "P1", Category = "Apples"},
                new Product{ ProductID = 2, Name = "P2", Category = "Apples"},
                new Product{ ProductID = 3, Name = "P3", Category = "Plums"},
                new Product{ ProductID = 4, Name = "P4", Category = "Oranges"}
            }).AsQueryable());

            var target = new NavigationMenuViewComponent(mock.Object);
            //Act
            var result = (target.Invoke() as ViewViewComponentResult).ViewData.Model as IEnumerable<string>;
            //Assert
            Assert.True(Enumerable.SequenceEqual(new string[] { "Apples", "Oranges", "Plums" }, result)); // Asserting that duplicates are removed and the alphabetical ordering is imposed.
        }

        [Fact]
        public void Indicates_Selected_Category()
        {
            //Arrange
            string categoryToSelect = "Apples";
            var mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product{ProductID = 1, Name = "P1", Category = "Apples"},
                new Product{ProductID = 2, Name = "P2", Category = "Oranges"}
            }).AsQueryable());

            var target = new NavigationMenuViewComponent(mock.Object);

            target.ViewComponentContext = new ViewComponentContext { ViewContext = new ViewContext { RouteData = new RouteData()} };
            target.RouteData.Values["category"] = categoryToSelect;

            //Act
            var result = (target.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"] as string;

            //Assert
            Assert.Equal(categoryToSelect, result);
        }
    }
}
