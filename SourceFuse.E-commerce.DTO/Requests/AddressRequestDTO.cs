using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.DTO.Requests
{
    [DisplayName("Address")]
    public class AddressRequestDTO
    {
        public int Id { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public int CustomerId { get; set; }
        public CustomerRequestDTO Customer { get; set; }
    }
}
