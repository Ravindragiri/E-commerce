using SourceFuse.E_commerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Business.Interfaces
{
    public interface IOrderItemBusiness
    {
        IEnumerable<OrderItem> GetAll();
        public OrderItem GetById(int id);
        bool Add(OrderItem item);
        bool Update(OrderItem item);
        bool Delete(long id);
    }
}
