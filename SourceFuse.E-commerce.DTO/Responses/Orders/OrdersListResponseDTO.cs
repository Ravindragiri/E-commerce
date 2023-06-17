using SourceFuse.E_commerce.DTO.Responses.BaseResponses;
using SourceFuse.E_commerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.DTO.Responses.Orders
{
    public class OrdersListResponseDTO : PagedResponseDTO
    {
        public IEnumerable<OrderResponseDTO> Orders { get; set; }

        public static OrdersListResponseDTO Build(List<Order> orders,
            string basePath,
            int currentPage, int pageSize, int totalItemCount)
        {
            List<OrderResponseDTO> orderDtos = new List<OrderResponseDTO>(orders.Count);
            foreach (var order in orders)
            {
                orderDtos.Add(OrderResponseDTO.Build(order));
            }

            return new OrdersListResponseDTO
            {
                PageMeta = new PageMetaDTO(orders.Count, basePath, currentPageNumber: currentPage, requestedPageSize: pageSize,
                    totalItemCount: totalItemCount),
                Orders = orderDtos
            };
        }
    }
}
