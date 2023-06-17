using Microsoft.EntityFrameworkCore;
using SourceFuse.E_commerce.Common.Exceptions;
using SourceFuse.E_commerce.DTO.Requests;
using SourceFuse.E_commerce.Entities;
using SourceFuse.E_commerce.Entities.Identity;
using SourceFuse.E_commerce.Persistence.Context;
using SourceFuse.E_commerce.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Persistence
{
    public class OrderPersist: IOrderPersist
    {
        private readonly EcommerceContext _context;
        private readonly IProductPersist _productsPersist;

        public OrderPersist(EcommerceContext context, IProductPersist productsPersist)
        {
            _context = context;
            _productsPersist = productsPersist;
        }

        public async Task<Tuple<int, List<Order>>> FetchPageFromUser(ApplicationUser user = null, int page = 1,
            int pageSize = 5)
        {
            // It is actually a IIncludableQueryable<Order,Product> but to filter by user without creating other variable we "downgrade it"
            // to IQeuryable<Order>
            IQueryable<Order> queryable = _context.Orders.Include(o => o.OrderItems).ThenInclude(ci => ci.Product);
            if (user != null)
            {
                queryable = queryable.Where(o => o.User == user);
            }

            var count = queryable.Count();
            List<Order> orders = await queryable.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return Tuple.Create(count, orders);
        }

        public async Task<Tuple<int, List<Order>>> FetchPageFromUser(long userId, int page = 1, int pageSize = 5)
        {
            IQueryable<Order> queryable = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(orderItem => orderItem.Product)
                .Include(o => o.Address)
                .Where(o => o.User.Id == userId);


            var count = queryable.Count();
            List<Order> orders = await queryable.Skip((page - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            return Tuple.Create(count, orders);
        }

        public async Task<Order> FetchById(long id, bool includeAddress = false, bool includeUser = false,
            bool includeOrderItems = false)
        {
            IQueryable<Order> queryable = _context.Orders.Where(o => o.Id == id);
            if (includeUser)
                queryable = queryable.Include(o => o.User);

            if (includeAddress)
                queryable = queryable.Include(o => o.Address);

            if (includeOrderItems)
                queryable = queryable.Include(o => o.OrderItems);


            return await queryable.FirstAsync();
        }


        public async Task<List<Order>> FetchAllFromUserId(long userId)
        {
            return await _context.Orders.Where(o => o.User.Id == userId).ToListAsync();
        }


        // Not Used
        public async Task<IEnumerable<Order>> FetchFromUserNotUsed(long userId)
        {
            return await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
        }

        // Not Used
        public Order FetchByIdNotUser(long id) => _context.Orders
            .Include(o => o.OrderItems).First(o => o.Id == id);

        public int GetTotalSum(Order order)
        {
            var sum = 0;
            foreach (var item in order.OrderItems)
            {
                sum += (item.Product.Price * item.Quantity);
            }

            return sum;
        }

        public async Task<Order> Create(CreateOrderRequestDTO form, ApplicationUser user)
        {
            if (form.OrderItems == null)
                return null;
            Order order = new Order();

            if (user != null)
                order.User = user;


            List<OrderItem> orderItems = new List<OrderItem>();
            IEnumerable<long> productIds = form.OrderItems.Select(ci => ci.ProductId);


            List<Product> products = await _productsPersist.FetchByIdInRetrieveNamePriceAndSlug(productIds);
            var address = await _context.Address.FirstOrDefaultAsync(x => x.ApplicationUserId == user.Id);

            if (products.Count != form.OrderItems.Count)
                return null;

            for (int i = 0; i < products.Count; i++)
            {
                var product = products[i];
                orderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = form.OrderItems[i].Quantity,
                    Price = product.Price,
                    Name = product.Name,
                    Slug = product.Slug,
                    User = user
                });
            }

            order.OrderItems = orderItems;
            order.CardNumber = form.CardNumber;
            order.TrackingNumber = form.TrackingNumber;
            order.Comments = form.Comments;

            order.AddressId = address.Id;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public void Create(Order order)
        {
            _context.AttachRange(order.OrderItems.Select(l => l.Product));
            if (order.Id == 0)
            {
                _context.Orders.Add(order);
            }

            _context.SaveChanges();
        }

        public async Task AddOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }


        public async Task Delete(long id)
        {
            Order order = await FetchById(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}
