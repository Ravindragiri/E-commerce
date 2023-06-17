using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.DTO.Responses.User
{
    public class ChangePasswordResponseDTO
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string UserId { get; set; }
    }
}
