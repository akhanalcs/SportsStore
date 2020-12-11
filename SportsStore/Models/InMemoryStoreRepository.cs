using System;
using System.Linq;

namespace SportsStore.Models
{
    public class InMemoryStoreRepository : IStoreRepository
    {
        public IQueryable<Product> Products
        {
            get
            {
                return (new Product[]
                {
                    new Product
                    {
                        ProductID = 1,
                        Name = "Kayak",
                        Description = "A boat for one person",
                        Category = "Watersports",
                        Price = 275
                    },
                    new Product
                    {
                        ProductID = 2,
                        Name = "Lifejacket",
                        Description = "Protective and fashionable",
                        Category = "Watersports",
                        Price = 48.95m
                    },
                    new Product
                    {
                        ProductID = 3,
                        Name = "Soccer Ball",
                        Description = "FIFA-approved size and weight",
                        Category = "Soccer",
                        Price = 19.50m
                    },
                    new Product
                    {
                        ProductID = 4,
                        Name = "Corner Flags",
                        Description = "Give your playing field a professional touch",
                        Category = "Soccer",
                        Price = 34.95m
                    },
                    new Product
                    {
                        ProductID = 5,
                        Name = "Stadium",
                        Description = "Flat-packed 35,000-seat stadium",
                        Category = "Soccer",
                        Price = 79500
                    },
                    new Product
                    {
                        ProductID = 6,
                        Name = "Thinking Cap",
                        Description = "Improve brain efficiency by 75%",
                        Category = "Chess",
                        Price = 16
                    },
                    new Product
                    {
                        ProductID = 7,
                        Name = "Unsteady Chair",
                        Description = "Secretly give your opponent a disadvantage",
                        Category = "Chess",
                        Price = 29.95m
                    },
                    new Product
                    {
                        ProductID = 8,
                        Name = "Human Chess Board",
                        Description = "A fun game for the family",
                        Category = "Chess",
                        Price = 75
                    },
                    new Product
                    {
                        ProductID = 9,
                        Name = "Bling-Bling King",
                        Description = "Gold-plated, diamond-studded King",
                        Category = "Chess",
                        Price = 1200
                    }
                }).AsQueryable();
            }
        }

        public void CreateProduct(Product p)
        {
            throw new NotImplementedException();
        }

        public void DeleteProduct(Product p)
        {
            throw new NotImplementedException();
        }

        public void SaveProduct(Product p)
        {
            throw new NotImplementedException();
        }
    }
}
