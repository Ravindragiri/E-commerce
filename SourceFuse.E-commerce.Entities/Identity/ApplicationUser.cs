using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SourceFuse.E_commerce.Entities.Identity
{
    public class ApplicationUser : IdentityUser<long>
    {
        public ApplicationUser()
        {
            // Orders = new HashSet<Order>();
        }

        public ApplicationUser(string Email, string FirstName, string LastName)
        {

            this.Email = Email;
            this.UserName = $"{FirstName}.{LastName}";
            this.FirstName = FirstName;
            this.LastName = LastName;

            Isactive = true;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CountryCode { get; set; }
        public string? Phone { get; set; }
        //public List<UserRole> UserRoles { get; set; }
        //public long? CustomerId { get; set; }
        public Customer Customer { get; set; }
        public long GenderId { get; set; }
        public string? Password { get; set; }
        public bool IsVerified { get; set; }
        public bool Isactive { get; set; }


        public virtual UserGender Gender { get; set; }
        public Address Address  { get; set; }
        //public long AddressId { get; set; }
        public virtual ICollection<AppUserRole> UserRoleMappings { get; set; }
        //public virtual ICollection<UserRoleMapping> UserRoleMappings { get; set; }

        /// <summary>
        /// Navigation property for the roles this user belongs to.
        /// </summary>
        //public virtual ICollection<ApplicationRole> Roles { get; set; } = new List<ApplicationRole>();


        //public virtual ICollection<IdentityUserRole<int>> Roless { get; } = new List<IdentityUserRole<int>>();

        /// <summary>
        /// Navigation property for the claims this user possesses.
        /// </summary>
        public virtual ICollection<IdentityUserClaim<long>> Claims { get; } = new List<IdentityUserClaim<long>>();

        /// <summary>
        /// Navigation property for this users login accounts.
        /// </summary>
        public virtual ICollection<IdentityUserLogin<long>> Logins { get; } = new List<IdentityUserLogin<long>>();
        public virtual ICollection<Order> Orders { get; set; }

    }
}
