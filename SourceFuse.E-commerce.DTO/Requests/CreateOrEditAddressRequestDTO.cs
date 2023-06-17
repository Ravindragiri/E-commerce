using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.DTO.Requests
{
    public class CreateOrEditAddressRequestDTO
    {
        public string Lastname { get; set; }
        public string Country { get; set; }
        public string FirstName { get; set; }
        public string City { get; set; }
        public string StreetAddress { get; set; }
        public string ZipCode { get; set; }
    }
}
