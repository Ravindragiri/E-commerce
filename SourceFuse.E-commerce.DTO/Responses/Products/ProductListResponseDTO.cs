using SourceFuse.E_commerce.DTO.Responses.BaseResponses;
using SourceFuse.E_commerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.DTO.Responses.Products
{
    public class ProductListResponseDTO : PagedResponseDTO
    {
        public IEnumerable<ProductSummaryResponseDTO> Products { get; set; }
        //    public int SortBy {get; set;}


        public static ProductListResponseDTO Build(List<Product> products,
            string basePath,
            int currentPage, int pageSize, int totalItemCount)
        {
            List<ProductSummaryResponseDTO> productSummaryDtos = new List<ProductSummaryResponseDTO>(products.Count);
            foreach (var product in products)
            {
                productSummaryDtos.Add(ProductSummaryResponseDTO.Build(product));
            }

            return new ProductListResponseDTO
            {
                PageMeta = new PageMetaDTO(products.Count, basePath, currentPageNumber: currentPage, requestedPageSize: pageSize,
                    totalItemCount: totalItemCount),
                Products = productSummaryDtos
            };
        }
    }
}
