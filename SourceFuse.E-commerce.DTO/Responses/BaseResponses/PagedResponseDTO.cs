using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.DTO.Responses.BaseResponses
{
    public class PagedResponseDTO : SuccessResponseDTO
    {
        public PageMetaDTO PageMeta { get; set; }
    }
}
