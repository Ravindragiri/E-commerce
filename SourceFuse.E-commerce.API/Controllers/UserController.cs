using AutoMapper;
using Bogus.DataSets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SourceFuse.E_commerce.API.Extensions;
using SourceFuse.E_commerce.Business.Interfaces;
using SourceFuse.E_commerce.DTO.Requests;
using SourceFuse.E_commerce.DTO.Responses.Contact;
using SourceFuse.E_commerce.Entities;
using SourceFuse.E_commerce.Entities.Identity;

namespace SourceFuse.E_commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness _userBusiness;
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserController(IUserBusiness userBusiness, 
            ILogger<UserController> logger,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userBusiness = userBusiness;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("Sign-up")]
        /*[Authorize]*/
        public IActionResult SignUp(SignUpModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _userBusiness.UserSignUp(user);
                _logger.LogInformation("-------------API Respond Successfully-------------");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("Sign-in")]
        public IActionResult Login(LoginModel credentials)
        {
            try
            {
                var details = _userBusiness.Login(credentials);
                _logger.LogInformation("-------------API Respond Successfully-------------");
                return Ok(details);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllUsers")]
        [Authorize(Roles = "ROLE_ADMIN")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var Result = _userBusiness.GetAllUsers();
                _logger.LogInformation("-------------API Respond Successfully-------------");
                return Ok(Result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("User/{id}")]
        [Authorize(Roles = "ROLE_ADMIN")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                var result = _userBusiness.GetUserById(id);
                _logger.LogInformation("-------------API Respond Successfully-------------");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressRequestDTO>> GetUserAddress()
        {
            var user = await _userManager.FindByEmailWithAddressAsync(User);
            var address = _mapper.Map<AddressRequestDTO>(user.Address);

            return Ok(address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressResponseDTO>> UpdateUserAddress(AddressRequestDTO address)
        {
            var user = await _userManager.FindByEmailWithAddressAsync(User);
            user.Address = _mapper.Map<SourceFuse.E_commerce.Entities.Address>(address);
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return Ok(_mapper.Map<AddressResponseDTO>(user.Address));

            return BadRequest("Problem updating the user");
        }
    }
}
