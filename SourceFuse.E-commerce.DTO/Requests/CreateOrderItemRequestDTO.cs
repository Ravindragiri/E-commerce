using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.DTO.Requests
{
    public class CreateOrderItemRequestDTO
    {
        public long ProductId { get; set; }
        public long OrderId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public long? UserId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
    }
}
