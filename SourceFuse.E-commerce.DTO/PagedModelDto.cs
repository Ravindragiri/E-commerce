using SourceFuse.E_commerce.Entities.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using LinkedResource = SourceFuse.E_commerce.Entities.Links.LinkedResource;

namespace SourceFuse.E_commerce.DTO
{
    public record PagedModelDto : ILinkedResource
    {
        public int CurrentPage { get; init; }

        public int TotalItems { get; init; }

        public int TotalPages { get; init; }


        public IDictionary<LinkedResourceType, LinkedResource> Links { get; set; }

    }
}
