using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.DTO.Requests
{
    public class LoginRequestDTO
    {
        [Required]
        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

        [Required]
        [JsonProperty(PropertyName = "password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
