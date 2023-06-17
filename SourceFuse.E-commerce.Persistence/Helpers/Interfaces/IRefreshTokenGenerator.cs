using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Persistence.Helpers.Interfaces
{
    public interface IRefreshTokenGenerator
    {
        public string GenerateToken();
    }
}
