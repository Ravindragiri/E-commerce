using Microsoft.EntityFrameworkCore;
using SourceFuse.E_commerce.Entities.Identity;
using SourceFuse.E_commerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceFuse.E_commerce.Persistence.Context;
using SourceFuse.E_commerce.Persistence.Interfaces;
using SourceFuse.E_commerce.Common.Exceptions;

namespace SourceFuse.E_commerce.Persistence
{
    public class OrderItemPersist : IOrderItemPersist
    {
        private readonly EcommerceContext _context;

        public OrderItemPersist(EcommerceContext context)
        {
            _context = context;
        }

        public async Task<Tuple<int, List<OrderItem>>> FetchPageFromUser(int page = 1,
            int pageSize = 5)
        {
            IQueryable<OrderItem> queryable = _context.OrderItems;

            var count = queryable.Count();
            List<OrderItem> orders = await queryable.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return Tuple.Create(count, orders);
        }

        public async Task<OrderItem> FetchById(long id)
        {
            IQueryable<OrderItem> queryable = _context.OrderItems.Where(o => o.OrderId == id);

            return await queryable.FirstAsync();
        }

        public async Task<bool> Update(OrderItem orderItem)
        {
           var orderItemResult =  await _context.OrderItems.FirstOrDefaultAsync(e => e.ProductId == orderItem.ProductId && e.OrderId == orderItem.OrderId);
            if(orderItemResult == null)
            {
                throw new ResourceNotFoundException();
            }

            orderItemResult.Quantity = orderItem.Quantity;
            orderItemResult.Price = orderItem.Price;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(long id)
        {
            OrderItem order = await FetchById(id);
            if (order != null)
            {
                _context.OrderItems.Remove(order);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }


    }
}
