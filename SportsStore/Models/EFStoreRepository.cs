using System;
using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Models
{
    public class EFStoreRepository : IStoreRepository
    {
        private StoreDbContext context;
        public EFStoreRepository(StoreDbContext ctx) // To talk to SQL server, This repository needs DbContext that's why it's injected here.
        {
            context = ctx;
        }

        public IQueryable<Product> Products
        {
            get
            {
                return context.Products;
            }
        }

        public void CreateProduct(Product p)
        {
            context.Add(p);
            context.SaveChanges();
        }

        public void DeleteProduct(Product p)
        {
            context.Remove(p);
            context.SaveChanges();
        }

        public void SaveProduct(Product p)
        {
            context.SaveChanges();
        }
    }
}
