using SourceFuse.E_commerce.DTO.Responses.BaseResponses;
using SourceFuse.E_commerce.DTO.Responses.Categories;
using SourceFuse.E_commerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.DTO.Responses.Products
{
    public class ProductDetailsResponseDTO: SuccessResponseDTO
    {
        public long Id { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime ModifiedAt { get; set; }
        public DateTime PublishedAt { get; set; }
        public IEnumerable<string> Categories { get; set; }

        public static ProductDetailsResponseDTO Build(Product product)
        {
            return new ProductDetailsResponseDTO
            {
                Id = product.Id,
                Name = product.Name,
                Slug = product.Slug,
                Description = product.Description,
                PublishedAt = product.PublishAt,
                Categories = CategoryOnlyNameResponseDTO.BuildAsStringList(product.ProductCategories),
            };
        }
    }
}
