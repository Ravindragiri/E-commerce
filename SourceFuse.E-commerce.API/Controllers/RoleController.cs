using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SourceFuse.E_commerce.Business.Interfaces;
using SourceFuse.E_commerce.Entities.Identity;

namespace SourceFuse.E_commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleBusiness _roleBusiness;
        private readonly ILogger<RoleController> _logger;
        public RoleController(IRoleBusiness roleBusiness, ILogger<RoleController> logger)
        {
            _logger = logger;
            _roleBusiness = roleBusiness;
        }

        [HttpPost]
        [Route("AddRole")]
        [Authorize(Roles = "ROLE_ADMIN")]
        public IActionResult AddRole(RoleModel model)
        {
            try
            {
                var Result = _roleBusiness.AddRole(model);
                _logger.LogInformation("-------------API Respond Successfully-------------");
                return Ok(Result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }


        [HttpDelete]
        [Route("DeleteRole")]
        [Authorize(Roles = "ROLE_ADMIN")]
        public IActionResult DeleteRole(DeleteRoleModel model)
        {
            try
            {
                var Result = _roleBusiness.DeleteRole(model);
                _logger.LogInformation("-------------API Respond Successfully-------------");
                return Ok(Result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }


        [HttpGet]
        [Route("ShowAllRoles")]
        public IActionResult ShowAllRoles()
        {
            try
            {
                var Result = _roleBusiness.GetAllRoles();
                _logger.LogInformation("-------------API Respond Successfully-------------");
                return Ok(Result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }
    }
}
