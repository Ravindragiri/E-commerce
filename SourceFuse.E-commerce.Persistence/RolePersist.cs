using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using SourceFuse.E_commerce.Entities.Identity;
using SourceFuse.E_commerce.Persistence.Context;
using SourceFuse.E_commerce.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.Persistence
{
    public class RolePersist: IRolePersist
    {
        private readonly ILogger<RolePersist> _logger;

        public RolePersist(ILogger<RolePersist> logger)
        {
            _logger = logger;
        }

        public bool AddRole(RoleModel model)
        {
            try
            {
                EcommerceContext db = new EcommerceContext();
                _logger.LogInformation("--------DB COnnection Established--------");
                var Role = new ApplicationRole(model.Role);

                db.Roles.Add(Role);
                db.SaveChanges();
                _logger.LogInformation("---------Role Added---------");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteRole(DeleteRoleModel model)
        {
            try
            {
                EcommerceContext db = new EcommerceContext();
                _logger.LogInformation("----------DB Connection Established---------");
                var role = db.Roles.FirstOrDefault(x => x.Id == model.RoleId);
                if (role == null)
                {
                    _logger.LogError("-------Invalid Role Id-------");
                    throw new Exception("Invalid RoleId");
                }

                var userCount = db.UserRoles.Count(x => x.RoleId == model.RoleId);

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (userCount == 0)
                        {
                            var DeleteRole = db.UserRoles.FirstOrDefault(x => x.RoleId == model.RoleId);
                            db.UserRoles.Remove(DeleteRole);
                            db.SaveChanges();

                            db.Roles.Remove(role);
                            db.SaveChanges();

                            transaction.Commit();

                            _logger.LogInformation("----------Role Removed--------");
                            return true;
                        }
                        else
                        {
                            _logger.LogInformation("----------------Role Can not be Deleted--------");
                            throw new Exception("There Is Many User With This Role. Please Remove All User With This Role Before Deleting This Role");
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Error occurred.");
                        throw new Exception(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
                throw new Exception(ex.Message);
            }
        }

        public List<ShowRoles> GetAllRoles()
        {
            try
            {
                EcommerceContext db = new EcommerceContext();
                _logger.LogInformation("--------DB Connection Established---------");
                List<ShowRoles> RoleList = new List<ShowRoles>();

                var AllRoles = db.UserRoles.Select(x => new ShowRoles { RoleId = x.RoleId, Role = x.Role.Name });

                foreach (var Role in AllRoles)
                {
                    RoleList.Add(Role);
                }

                foreach (var Role in RoleList)
                {
                    Role.UserCount = db.UserRoles.Count(x => Role.RoleId == x.RoleId);
                }

                _logger.LogInformation("---------Roles Retrieved---------");
                return RoleList;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
                throw new Exception(ex.Message);
            }
        }
    }
}
