using SourceFuse.E_commerce.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.DTO.Requests
{
    public class CreateOrderRequestDTO
    {
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        public string CardNumber { get; set; }

        //public string Address { get; set; }
        //public long? AddressId { get; set; }

        public long UserId { get; set; }
        //public string City { get; set; }
        //public string Country { get; set; }
        //public string PhoneNumber { get; set; }
        //public String ZipCode { get; set; }
        public string TrackingNumber { get; set; }
        public string Comments { get; set; }

        public Collection<CartItemRequestDTO> OrderItems { get; set; }
    }
}
