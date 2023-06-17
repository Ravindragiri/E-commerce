using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SourceFuse.E_commerce.Entities.Identity;
using System.Security.Claims;

namespace SourceFuse.E_commerce.API.Extensions
{
    public static class UserManagerExtension
    {
        public static async Task<ApplicationUser> FindByEmailWithAddressAsync(this UserManager<ApplicationUser> input,
                                                                           ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);
            return await input.Users.Include(x => x.Address).SingleOrDefaultAsync(x => x.Email == email);

        }
    }
}
