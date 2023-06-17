using SourceFuse.E_commerce.DTO.Responses.BaseResponses;
using SourceFuse.E_commerce.DTO.Responses.Contact;
using SourceFuse.E_commerce.DTO.Responses.OrderItems;
using SourceFuse.E_commerce.DTO.Responses.User;
using SourceFuse.E_commerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.DTO.Responses.Orders
{
    public class OrderDetailsResponseDTO : SuccessResponseDTO
    {
        public long Id { get; set; }
        public string TrackingNumber { get; set; }
        public UserBasicEmbeddedInfoResponseDTO User { get; set; }
        public AddressResponseDTO Address { get; set; }
        public ICollection<OrderItemResponseDTO> OrderItems { get; set; }

        public static OrderDetailsResponseDTO Build(Order order, bool includeUser = false)
        {
            List<OrderItemResponseDTO> orderItemDtos = new List<OrderItemResponseDTO>(order.OrderItems.Count);

            foreach (var orderItem in order.OrderItems)
            {
                orderItemDtos.Add(OrderItemResponseDTO.Build(orderItem));
            }

            var dto = new OrderDetailsResponseDTO
            {
                Id = order.Id,
                TrackingNumber = order.TrackingNumber,
                Address = AddressResponseDTO.Build(order.Address),
                OrderItems = orderItemDtos
            };

            return dto;
        }
    }
}
