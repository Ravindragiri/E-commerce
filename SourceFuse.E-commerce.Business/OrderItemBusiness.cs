using SourceFuse.E_commerce.Business.Interfaces;
using SourceFuse.E_commerce.Entities;
using SourceFuse.E_commerce.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Business
{
    public class OrderItemBusiness: IOrderItemBusiness
    {
        private readonly IGenericPersist<OrderItem> _genericPersist;
        private readonly IOrderItemPersist _orderItemPersist;
        public OrderItemBusiness(IGenericPersist<OrderItem> genericPersist,
            IOrderItemPersist orderItemPersist) 
        {
            _genericPersist = genericPersist;
            _orderItemPersist  = orderItemPersist;
        }

        public bool Add(OrderItem item)
        {
            _genericPersist.Add(item);
            return _genericPersist.SaveChangesAsync().Result;
        }

        public bool Delete(long id)
        {
            _genericPersist.DeleteById(id);
            return _genericPersist.SaveChangesAsync().Result;
        }

        public IEnumerable<OrderItem> GetAll()
        {
            return _genericPersist.GetAll();
        }

        public OrderItem GetById(int id)
        {
            return _orderItemPersist.FetchById(id).Result;
        }

        public bool Update(OrderItem item)
        {
            return _orderItemPersist.Update(item).Result;
        }
    }
}
