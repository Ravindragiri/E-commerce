using SourceFuse.E_commerce.Entities.Identity;
using SourceFuse.E_commerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Persistence.Interfaces
{
    public interface IUserPersist
    {
        public string UserSignUp(SignUpModel user);
        public UserGender GetGender(long id);
        public ApplicationRole GetRole(long id);
        public UserDetailsModel Login(LoginModel credentials);
        public List<UserDetailsModel> GetAllUsers();
        public UserDetailsModel GetUserById(int id);
    }
}
