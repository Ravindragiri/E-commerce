using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Entities.Identity
{
    public class TokenModel
    {
        public int UserId { get; protected set; }

        public string Email { get; protected set; }

        public string Token { get; protected set; }

        public DateTime Expires { get; protected set; }

        public string RefreshToken { get; protected set; }

        public string UserType { get; protected set; }

        public TokenModel(int userId, string email, string token, DateTime expires, string refreshToken, string type)
        {
            UserId = userId;
            Email = email;
            Token = token;
            Expires = expires;
            RefreshToken = refreshToken;
            UserType = type;
        }
    }
}
