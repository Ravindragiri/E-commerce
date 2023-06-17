using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SourceFuse.E_commerce.Entities.Identity;

namespace SourceFuse.E_commerce.Entities
{
    public class Order
    {
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }

        public long Id { get; set; }
        [BindNever] public ICollection<OrderItem> OrderItems { get; set; }

        [Required(ErrorMessage = "Please enter the address to ship to")]
        public string CardNumber { get; set; }
        public ApplicationUser User { get; set; }

        public long? UserId { get; set; }
        // public OrderInfo OrderInfo { get; set; }

        public Address Address { get; set; }
        public long AddressId { get; set; }
        public string TrackingNumber { get; set; }
        [NotMapped] public decimal Sum { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedAt { get; set; }
        private DateTime UpdatedAt { get; set; }
    }
}
