using SourceFuse.E_commerce.Business.Interfaces;
using SourceFuse.E_commerce.Entities;
using SourceFuse.E_commerce.Entities.Identity;
using SourceFuse.E_commerce.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Business
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserPersist _userPersist;

        public UserBusiness(IUserPersist userPersist) 
        {
            _userPersist = userPersist;
        }
        
        public List<UserDetailsModel> GetAllUsers()
        {
            return _userPersist.GetAllUsers();
        }

        public UserGender GetGender(int id)
        {
            return _userPersist.GetGender(id);
        }

        public ApplicationRole GetRole(int id)
        {
            return _userPersist.GetRole(id);
        }

        public UserDetailsModel GetUserById(int id)
        {
            return _userPersist.GetUserById(id);
        }

        public UserDetailsModel Login(LoginModel credentials)
        {
            return _userPersist.Login(credentials);
        }

        public string UserSignUp(SignUpModel user)
        {
            return _userPersist.UserSignUp(user);
        }
    }
}
