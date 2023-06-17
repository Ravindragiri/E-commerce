using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Entities.Identity
{
    public class RoleModel
    {
        [Required]
        public string Role { get; set; }
    }

    public class DeleteRoleModel
    {
        [Required]
        public long RoleId { get; set; }
    }

    public class ShowRoles
    {
        public long RoleId { get; set; }
        public string Role { get; set; }
        public int UserCount { get; set; }
    }
}
