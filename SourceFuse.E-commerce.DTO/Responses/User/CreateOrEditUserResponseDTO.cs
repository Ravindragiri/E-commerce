using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.DTO.Responses.User
{
    public class CreateOrEditUserResponseDTO
    {
        public long Id { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
    }
}
