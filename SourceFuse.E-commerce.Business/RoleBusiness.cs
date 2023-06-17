using SourceFuse.E_commerce.Business.Interfaces;
using SourceFuse.E_commerce.Entities.Identity;
using SourceFuse.E_commerce.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Business
{
    public class RoleBusiness : IRoleBusiness
    {
        private readonly IRolePersist _rolePersist;
        public RoleBusiness(IRolePersist rolePersist) 
        {
            _rolePersist = rolePersist;
        }
        public bool AddRole(RoleModel model)
        {
            return _rolePersist.AddRole(model);
        }

        public bool DeleteRole(DeleteRoleModel model)
        {
            return _rolePersist.DeleteRole(model);
        }

        public List<ShowRoles> GetAllRoles()
        {
            return _rolePersist.GetAllRoles();
        }
    }
}
