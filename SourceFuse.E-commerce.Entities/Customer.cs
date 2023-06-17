using SourceFuse.E_commerce.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Entities
{
    public class Customer
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        //public Address Address { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public ApplicationUser User { get; set; }
        public long? ApplicationUserId { get; set; }

        [Timestamp]
        public byte[]? ModifiedAt { get; set; }
    }
}
