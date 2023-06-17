using SourceFuse.E_commerce.Business.Interfaces;
using SourceFuse.E_commerce.DTO.Requests;
using SourceFuse.E_commerce.Entities;
using SourceFuse.E_commerce.Entities.Identity;
using SourceFuse.E_commerce.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Business
{
    public class OrderBusiness : IOrderBusiness
    {
        private readonly IOrderPersist _orderPersist;

        public OrderBusiness(IOrderPersist orderPersist) 
        {
            this._orderPersist = orderPersist;
        }

        public async Task<Order> Create(CreateOrderRequestDTO form, ApplicationUser user)
        {
            return await _orderPersist.Create(form, user);
        }

        public void Create(Order order)
        {
            _orderPersist.Create(order);
        }

        public async Task Delete(long id)
        {
            await _orderPersist.Delete(id);
        }

        public async Task<List<Order>> FetchAllFromUserId(long userId)
        {
            return await _orderPersist.FetchAllFromUserId(userId);
        }

        public async Task<Order> FetchById(long id, bool includeAddress = false, bool includeUser = false, bool includeOrderItems = false)
        {
            return await _orderPersist.FetchById(id, includeAddress, includeUser, includeOrderItems);
        }

        public async Task<Tuple<int, List<Order>>> FetchPageFromUser(ApplicationUser user = null, int page = 1, int pageSize = 5)
        {
            return await _orderPersist.FetchPageFromUser(user, page, pageSize);
        }

        public async Task<Tuple<int, List<Order>>> FetchPageFromUser(long userId, int page = 1, int pageSize = 5)
        {
            return await _orderPersist.FetchPageFromUser(userId, page, pageSize);
        }

        public int GetTotalSum(Order order)
        {
            return _orderPersist.GetTotalSum(order);
        }
    }
}
