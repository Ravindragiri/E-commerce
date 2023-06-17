using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SourceFuse.E_commerce.Common.Encript_Decrypt;
using SourceFuse.E_commerce.Entities;
using SourceFuse.E_commerce.Entities.Identity;
using SourceFuse.E_commerce.Persistence.Context;
using SourceFuse.E_commerce.Persistence.Helpers.Interfaces;
using SourceFuse.E_commerce.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace SourceFuse.E_commerce.Persistence
{
    public class UserPersist : IUserPersist
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _context;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly ILogger<UserPersist> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserPersist(IHttpContextAccessor context,
            IConfiguration configuration,
            IRefreshTokenGenerator refreshTokenGenerator,
            ILogger<UserPersist> logger,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
            _refreshTokenGenerator = refreshTokenGenerator;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public UserGender GetGender(long id)
        {
            try
            {
                EcommerceContext db = new EcommerceContext();
                _logger.LogInformation("---------DB Connection Established----------");
                var gender = db.UserGenders.FirstOrDefault(x => x.Id == id);
                _logger.LogInformation("---------Gender Retrieved--------");
                return gender;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
                throw new Exception(ex.Message);
            }
        }

        public ApplicationRole GetRole(long id)
        {
            try
            {
                EcommerceContext db = new EcommerceContext();
                _logger.LogInformation("---------DB Connection Established----------");
                var role = db.Roles.FirstOrDefault(x => x.Id == id);
                _logger.LogInformation("---------Role Retrieved--------");
                return role;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
                throw new Exception(ex.Message);
            }
        }

        public ApplicationRole GetUserRole(long userId)
        {
            try
            {
                EcommerceContext db = new EcommerceContext();
                _logger.LogInformation("---------DB Connection Established----------");
                var role = db.UserRoles.FirstOrDefault(x => x.UserId == userId);
                _logger.LogInformation("---------User Role Retrieved--------");
                return GetRole(role.RoleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
                throw new Exception(ex.Message);
            }
        }

        public string UserSignUp(SignUpModel user)
        {
            try
            {
                EcommerceContext db = new EcommerceContext();
                _logger.LogInformation("---------DB Connection Established----------");

                var tempuser = new ApplicationUser()
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CountryCode = user.CountryCode,
                    Phone = user.Phone,
                    Email = user.Email,
                    GenderId = user.GenderId
                };

                var result = _userManager.CreateAsync(tempuser, user.Password).Result;
                if (result.Succeeded)
                {
                    ApplicationUser userFindResult = _userManager.FindByNameAsync(user.UserName).Result;
                    if (!_userManager.IsInRoleAsync(userFindResult, user.RoleName).Result)
                    {
                        var addToRoleResult = _userManager.AddToRoleAsync(userFindResult, user.RoleName).Result;
                    }
                }


                //_context.HttpContext.Session.SetInt32("Id", tempuser.Id);
                _logger.LogInformation("-----------User Registered--------");
                return "User Created";

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
                throw new Exception(ex.Message.ToString());
            }

        }

        public UserDetailsModel Login(LoginModel credentials)
        {
            try
            {
                EcommerceContext db = new EcommerceContext();
                _logger.LogInformation("----------DB Connection Established-----------");
                UserDetailsModel newuser = new UserDetailsModel();

                if (credentials != null && !string.IsNullOrWhiteSpace(credentials.UserName) && !string.IsNullOrWhiteSpace(credentials.Password))
                {
                    //var user = db.Users.FirstOrDefault(x => x.UserName == credentials.UserName && x.PasswordHash == Password.HashEncrypt(credentials.Password));
                    var signInResult = _signInManager.PasswordSignInAsync(credentials.UserName, credentials.Password,
                        false, false).Result;


                    if (!signInResult.Succeeded)
                    {
                        _logger.LogError("---------------Invalid Username or Password------------");
                        throw new Exception("Invalid UserName or Password");
                    }

                    ApplicationUser user = _userManager.Users.FirstOrDefaultAsync(u => u.UserName.Equals(credentials.UserName)).Result;

                    var jwt = _configuration.GetSection("Jwt").Get<Jwt>();
                    var userRoleMapping = db.UserRoles.First(x => x.UserId == user.Id);
                    //var userRole = db.UserRoles.Where(x => x.RoleId == userRoleMapping.RoleId);
                    var userRole = db.Roles.Where(x => x.Id == userRoleMapping.RoleId);

                    if (user != null)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(JwtRegisteredClaimNames.Sub,jwt.Subject),
                            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim("Password",credentials.Password),
                        };

                        foreach (var role in userRole)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role.Name));
                        }

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.key));

                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                                claims: claims,
                                issuer: jwt.Issuer,
                                audience: jwt.Audience,
                                signingCredentials: signIn,
                                expires: DateTime.Now.AddMinutes(60)
                                );
                        _logger.LogInformation("-----------JWT Token Generated-----------");

                        newuser = new UserDetailsModel()
                        {
                            UserName = user.UserName,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            Phone = "+" + user.CountryCode + user.Phone,
                            Gender = GetGender(user.GenderId),
                            Role = GetUserRole(user.Id),
                            Token = new JwtSecurityTokenHandler().WriteToken(token),
                            RefreshToken = _refreshTokenGenerator.GenerateToken()
                        };
                    }
                }
                _logger.LogInformation("-----------User Log In-----------");
                return newuser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception(ex.Message);
            }
        }

        public List<UserDetailsModel> GetAllUsers()
        {
            try
            {
                EcommerceContext db = new EcommerceContext();
                _logger.LogInformation("-------Db Connection Established----------");
                List<UserDetailsModel> list = new List<UserDetailsModel>();

                foreach (ApplicationUser user in db.Users)
                {
                    var printuser = new UserDetailsModel()
                    {
                        UserName = user.UserName,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Gender = GetGender(user.GenderId),
                        Phone = user.Phone,
                        Role = GetUserRole(user.Id),
                    };
                    list.Add(printuser);
                }
                _logger.LogInformation("-----------All User Retrieved-----------");
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
                throw new Exception(ex.Message);
            }
        }

        public UserDetailsModel GetUserById(int id)
        {
            try
            {
                EcommerceContext db = new EcommerceContext();
                _logger.LogInformation("-----------DB Connection Established----------");
                var user = db.Users.Where(x => x.Id == id).FirstOrDefault();
                var temp = new UserDetailsModel()
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Phone = user.Phone,
                    Email = user.Email,
                    Gender = GetGender(user.GenderId),
                    Role = GetUserRole(user.Id),
                };
                return temp;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
