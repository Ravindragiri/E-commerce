using SourceFuse.E_commerce.DTO.Responses.Categories;
using SourceFuse.E_commerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.DTO.Responses.Products
{
    public class ProductSummaryResponseDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }

        public int CommentsCount { get; set; }

        public List<string> Categories { get; set; }

        public DateTime PublishAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static ProductSummaryResponseDTO Build(Product product)
        {
            return new ProductSummaryResponseDTO
            {
                Id = product.Id,
                Name = product.Name,
                Slug = product.Slug,
                Price = product.Price,
                Stock = product.Stock,
                CommentsCount = product.CommentsCount,
                Categories = CategoryOnlyNameResponseDTO.BuildAsStringList(product.ProductCategories),
                PublishAt = product.PublishAt,
            };
        }
    }
}
