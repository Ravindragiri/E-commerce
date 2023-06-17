using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Entities.Identity
{
    public partial class UserGender
    {
        public UserGender()
        {
            Users = new HashSet<ApplicationUser>();
        }

        public long Id { get; set; }
        public string Gender { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}
