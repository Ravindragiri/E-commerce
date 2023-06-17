using SourceFuse.E_commerce.DTO.Responses.Contact;
using SourceFuse.E_commerce.DTO.Responses.User;
using SourceFuse.E_commerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.DTO.Responses.Orders
{
    public class OrderResponseDTO
    {
        public long Id { get; set; }
        public string TrackingNumber { get; set; }
        public UserBasicEmbeddedInfoResponseDTO User { get; set; }
        public AddressResponseDTO Address { get; set; }


        public static OrderResponseDTO Build(Order order, bool includeUser = false)
        {
            var dto = new OrderResponseDTO
            {
                Id = order.Id,
                TrackingNumber = order.TrackingNumber,
                Address = AddressResponseDTO.Build(order.Address),
            };

            if (includeUser)
                dto.User = UserBasicEmbeddedInfoResponseDTO.Build(order.User);

            return dto;
        }
    }
}
