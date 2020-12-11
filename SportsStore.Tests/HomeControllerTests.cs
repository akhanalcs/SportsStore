using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using Xunit;

namespace SportsStore.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void Can_Use_Repository()
        {
            //Arrange
            var mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product { ProductID = 1, Name = "P1" },
                new Product { ProductID = 2, Name = "P2" }
            }).AsQueryable<Product>());
            var controller = new HomeController(mock.Object);

            //Act
            var result = controller.Index(null).ViewData.Model as ProductsListViewModel;

            //Assert
            var prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P1", prodArray[0].Name);
            Assert.Equal("P2", prodArray[1].Name);
        }

        [Fact]
        public void Can_Paginate()
        {
            //Arrange
            var mock = new Mock<IStoreRepository>();

            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product { ProductID = 1, Name = "P1" },
                new Product { ProductID = 2, Name = "P2" },
                new Product { ProductID = 3, Name = "P3" },
                new Product { ProductID = 4, Name = "P4" },
                new Product { ProductID = 5, Name = "P5" }
            }).AsQueryable());

            var controller = new HomeController(mock.Object);
            controller.PageSize = 3;

            //Act
            var result = controller.Index(null, 2).ViewData.Model as ProductsListViewModel;

            //Assert
            var prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P4", prodArray[0].Name);
            Assert.Equal("P5", prodArray[1].Name);
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            //Arrange
            var mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"},
            }).AsQueryable());

            var controller = new HomeController(mock.Object)
            {
                PageSize = 3
            };

            //Act
            var result = controller.Index(null, 2).ViewData.Model as ProductsListViewModel;
            var pageInfo = result.PagingInfo;

            //Assert
            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
        }

        [Fact]
        public void Can_Filter_Products()
        {
            //Arrange
            var mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product{ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product{ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product{ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product{ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product{ProductID = 5, Name = "P5", Category = "Cat3"}
            }).AsQueryable());

            var controller = new HomeController(mock.Object);
            controller.PageSize = 3;
            //Act
            var result = (controller.Index("Cat2", 1).ViewData.Model as ProductsListViewModel).Products.ToArray();
            //Assert
            Assert.Equal(2, result.Length);
            Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.True(result[1].Name == "P4" && result[1].Category == "Cat2");
        }

        [Fact]
        public void Generate_Specific_Product_Count()
        {
            //Arrange
            var mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product{ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product{ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product{ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product{ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product{ProductID = 5, Name = "P5", Category = "Cat3"}
            }).AsQueryable());
            var controller = new HomeController(mock.Object);
            controller.PageSize = 3;

            Func<ViewResult, ProductsListViewModel> GetModel = viewResult => viewResult?.ViewData?.Model as ProductsListViewModel;

            //Act
            var result1 = GetModel(controller.Index("Cat1"))?.PagingInfo.TotalItems;
            var result2 = GetModel(controller.Index("Cat2"))?.PagingInfo.TotalItems;
            var result3 = GetModel(controller.Index("Cat3"))?.PagingInfo.TotalItems;
            var allResult = GetModel(controller.Index(null))?.PagingInfo.TotalItems;

            //Assert
            Assert.Equal(2, result1);
            Assert.Equal(2, result2);
            Assert.Equal(1, result3);
            Assert.Equal(5, allResult);
        }
    }
}
