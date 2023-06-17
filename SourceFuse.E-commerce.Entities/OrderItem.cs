using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceFuse.E_commerce.Entities.Identity;

namespace SourceFuse.E_commerce.Entities
{
    public class OrderItem
    {
        public long Id { get; set; }
        public Product Product { get; set; }
        public long ProductId { get; set; }
        public Order Order { get; set; }
        public long OrderId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public ApplicationUser User { get; set; }

        public long? UserId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
    }
}
