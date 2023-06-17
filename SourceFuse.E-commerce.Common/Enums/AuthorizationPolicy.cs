using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Common.Enums
{
    public enum AuthorizationPolicyEnum
    {
        ONLY_ADMIN, 
        ADMIN_AND_OWNER, 
        ONLY_OWNER, 
        AUTHENTICATED_USER, 
        ANY
    }
}
