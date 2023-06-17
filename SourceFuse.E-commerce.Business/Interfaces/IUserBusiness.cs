using SourceFuse.E_commerce.Entities.Identity;
using SourceFuse.E_commerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Business.Interfaces
{
    public interface IUserBusiness
    {
        public string UserSignUp(SignUpModel user);
        public UserGender GetGender(int id);
        public ApplicationRole GetRole(int id);
        public UserDetailsModel Login(LoginModel credentials);
        public List<UserDetailsModel> GetAllUsers();
        public UserDetailsModel GetUserById(int id);
    }
}
