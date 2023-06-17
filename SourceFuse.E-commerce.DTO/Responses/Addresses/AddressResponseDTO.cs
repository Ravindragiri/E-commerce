using SourceFuse.E_commerce.DTO.Responses.BaseResponses;
using SourceFuse.E_commerce.DTO.Responses.User;
using SourceFuse.E_commerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.DTO.Responses.Contact
{
    public class AddressResponseDTO : SuccessResponseDTO
    {
        public static AddressResponseDTO Build(Address address, bool includeUser = false)
        {
            var dto = new AddressResponseDTO
            {
                Id = address.Id,
                City = address.City,
                Country = address.Country,
                ZipCode = address.ZipCode,
                FirstName = address.FirstName,
                LastName = address.LastName,
                Address = address.StreetAddress
            };

            if (includeUser)
                dto.User = UserBasicEmbeddedInfoResponseDTO.Build(address.User);
            return dto;
        }

        public UserBasicEmbeddedInfoResponseDTO User { get; set; }

        public long Id { get; set; }
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
    }
}
