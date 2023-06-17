using Newtonsoft.Json;
using SourceFuse.E_commerce.DTO.Responses.Categories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.DTO.Responses.Products
{
    public class CreateOrEditProductResponseDTO
    {
        [JsonProperty(PropertyName = "name")]
        [Required]
        public string Name { get; set; }

        [JsonProperty(propertyName: "description")]
        [Required]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "categories")]
        public IEnumerable<CategoryOnlyNameResponseDTO> Categories { get; set; }
    }
}
