using Newtonsoft.Json;
using SourceFuse.E_commerce.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.DTO.Responses.User
{
    public class UserBasicEmbeddedInfoResponseDTO
    {
        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

        public long Id { get; set; }

        public static UserBasicEmbeddedInfoResponseDTO Build(ApplicationUser user)
        {
            return new UserBasicEmbeddedInfoResponseDTO
            {
                UserName = user.UserName,
                Id = user.Id
            };
        }
    }
}
