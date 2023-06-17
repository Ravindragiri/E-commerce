using SourceFuse.E_commerce.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Business.Interfaces
{
    public interface IRoleBusiness
    {
        public bool AddRole(RoleModel model);
        public bool DeleteRole(DeleteRoleModel model);
        public List<ShowRoles> GetAllRoles();
    }
}
