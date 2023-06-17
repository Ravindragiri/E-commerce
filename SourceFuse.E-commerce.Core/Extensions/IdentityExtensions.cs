using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Core.Extensions
{
    public static class IdentityExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            if (Guid.TryParse(principal.Identity.Name, out var userId))
            {
                return userId;
            }

            throw new Exception("Invalid User Claim Name.");
        }

        //public static UserType GetUserType(this ClaimsPrincipal claimsPrincipal)
        //{
        //    var claims = claimsPrincipal.Claims.ToList();

        //    if (!claims.Any())
        //    {
        //        return UserType.None;
        //    }

        //    var role = claims.FirstOrDefault(s => s.Type == ConstantValues.IdentityRoleName)?.Value ?? string.Empty;

        //    if (!Enum.TryParse(typeof(UserType), role, true, out var type))
        //    {
        //        return UserType.None;
        //    }

        //    return (UserType)type;
        //}
    }
}
