using SourceFuse.E_commerce.DTO.Requests;
using SourceFuse.E_commerce.Entities;
using SourceFuse.E_commerce.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Persistence.Interfaces
{
    public interface IOrderPersist
    {
        Task<Tuple<int, List<Order>>> FetchPageFromUser(ApplicationUser user = null, int page = 1, int pageSize = 5);
        Task<Order> Create(CreateOrderRequestDTO form, ApplicationUser user);
        int GetTotalSum(Order order);

        Task<Order> FetchById(long id, bool includeAddress = false, bool includeUser = false,
            bool includeOrderItems = false);

        Task Delete(long id);
        Task<List<Order>> FetchAllFromUserId(long userId);
        Task<Tuple<int, List<Order>>> FetchPageFromUser(long userId, int page = 1, int pageSize = 5);
        void Create(Order order);
    }
}
