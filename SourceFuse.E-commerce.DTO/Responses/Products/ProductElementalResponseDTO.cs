using SourceFuse.E_commerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.DTO.Responses.Products
{
    public class ProductElementalResponseDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }


        public static ProductElementalResponseDTO Build(Product product)
        {
            return new ProductElementalResponseDTO
            {
                Id = product.Id,
                Name = product.Name,
                Slug = product.Slug,
            };
        }
    }
}
