using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.DTO.Responses.BaseResponses
{
    public class SuccessResponseDTO : AppResponseDTO
    {
        public SuccessResponseDTO() : base(true)
        {
        }

        public SuccessResponseDTO(string message) : base(true, message)
        {
        }
    }
}
