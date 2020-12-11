using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private StoreDbContext context;

        public EFOrderRepository(StoreDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Order> Orders
        {
            get
            {
                return context.Orders.Include(o => o.Lines).ThenInclude(l => l.Product);
            }
        }

        public void SaveOrder(Order order)
        {
            var products = order.Lines.Select(l => l.Product);
            context.AttachRange(products);
            if(order.OrderID == 0)
            {
                context.Orders.Add(order);
            }
            context.SaveChanges(); //Page 205
        }
    }
}
