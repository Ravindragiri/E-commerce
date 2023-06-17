using SourceFuse.E_commerce.DTO.Responses.BaseResponses;
using SourceFuse.E_commerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.DTO.Responses.Contact
{
    public record AddressListResponseDTO : PagedModelDto
    {
        public List<Customer> Items { get; init; }
    }
}
