using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.DTO.Responses.BaseResponses
{
    public class ErrorResponseDTO: AppResponseDTO
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public ErrorResponseDTO() : base(false)
        {
        }

        public ErrorResponseDTO(string message) : base(false, message)
        {
        }
    }
}
