using SourceFuse.E_commerce.DTO.Responses.BaseResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.DTO.Responses.Categories
{
    public class CategoryResponseDTO : SuccessResponseDTO
    {
        public long Id { get; set; }
        public string Desc { get; set; }
        public string Name { get; set; }

        public static CategoryResponseDTO Build(Entities.Category category)
        {
            List<string> imageUrls = new List<string>();

            return new CategoryResponseDTO
            {
                Id = category.Id,
                Name = category.Name,
                Desc = category.Desc
            };
        }
    }
}
