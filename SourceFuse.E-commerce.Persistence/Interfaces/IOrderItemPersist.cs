using SourceFuse.E_commerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Persistence.Interfaces
{
    public interface IOrderItemPersist
    {
        Task<Tuple<int, List<OrderItem>>> FetchPageFromUser(int page = 1,
            int pageSize = 5);

        Task<OrderItem> FetchById(long id);

        Task<bool> Delete(long id);

        Task<bool> Update(OrderItem orderItem);
    }
}
