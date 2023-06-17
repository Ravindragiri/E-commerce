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
    public class ApplicationRole : IdentityRole<long>
    {
        public ApplicationRole(string name) : base(name)
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RoleId { get; set; }

        public int VersionNumber { get; set; }
        public string? Metadata { get; set; }
        public string? Slug { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public long CreateBy { get; set; }
        public long ModifyBy { get; set; }
        public int Status { get; set; }

        public virtual ICollection<AppUserRole> UserRoleMappings { get; set; }
    }
}
