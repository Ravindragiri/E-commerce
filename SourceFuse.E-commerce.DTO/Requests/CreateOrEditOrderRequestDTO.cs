using SourceFuse.E_commerce.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.DTO.Requests
{
    public class CreateOrEditOrderRequestDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }


        private string CardNumber { get; set; }

        //public string Address { get; set; }

        public long? AddressId { get; set; }

        public Address Address { get; set; }

        public string City { get; set; }


        public string Country { get; set; }

        public string PhoneNumber { get; set; }

        public String ZipCode { get; set; }

        public Collection<CartItemRequestDTO> CartItems { get; set; }
    }

    public class CartItemRequestDTO
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
