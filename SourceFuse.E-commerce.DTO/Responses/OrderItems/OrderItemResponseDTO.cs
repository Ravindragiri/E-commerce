using SourceFuse.E_commerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.DTO.Responses.OrderItems
{
    public class OrderItemResponseDTO
    {
        public static OrderItemResponseDTO Build(OrderItem orderItem)
        {
            return new OrderItemResponseDTO
            {
                Name = orderItem.Name,
                Price = orderItem.Price,
                Slug = orderItem.Slug
            };
        }

        public string Slug { get; set; }

        public int Price { get; set; }

        public string Name { get; set; }
    }
}
