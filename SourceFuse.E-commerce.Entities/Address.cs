using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceFuse.E_commerce.Entities.Identity;

namespace SourceFuse.E_commerce.Entities
{
    public class Address
    {
        public long Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? StreetAddress { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }

        public string? State { get; set; }
        public int Postalcode { get; set; }
        public string? ZipCode { get; set; }

        public ApplicationUser User { get; set; }
        public long? ApplicationUserId { get; set; } = null;
    }
}
