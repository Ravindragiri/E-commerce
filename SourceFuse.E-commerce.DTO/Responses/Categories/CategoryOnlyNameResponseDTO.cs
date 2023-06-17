using SourceFuse.E_commerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.DTO.Responses.Categories
{
    public class CategoryOnlyNameResponseDTO
    {
        public string Name { get; set; }

        public static List<string> BuildAsStringList(ICollection<ProductCategory> productCategories)
        {
            if (productCategories == null)
                return null;
            List<string> result = new List<string>(productCategories.Count);
            foreach (var productCategory in productCategories)
            {
                result.Add(productCategory.Category?.Name);
            }

            return result;
        }
    }
}
