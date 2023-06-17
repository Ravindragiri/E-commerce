using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SourceFuse.E_commerce.Entities.Identity
{
    public class AppUserRole : IdentityUserRole<long>
    {
        //public long Id { get; set; }
        public override long UserId { get; set; }
        public override long RoleId { get; set; }
        public ApplicationUser User { get; set; }
        public ApplicationRole Role { get; set; }
    }
}
