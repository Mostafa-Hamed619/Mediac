using MediacApi.Data.Entities;
using MediacApi.DTOs.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MediacApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;
        public AdminController(RoleManager<IdentityRole> roleManager,UserManager<User> userManager)
        {
            this._roleManager = roleManager;
            this._userManager = userManager;
        }

        [HttpGet("Get-Roles")]
        public async Task<ActionResult<IEnumerable<IdentityRole>>> getRole()
        {
            var Roles = await _roleManager.Roles.ToListAsync();

            return Ok(Roles);
        }

        [HttpPost("Add-Role")]
        public async Task<IActionResult> AddRole(addRoleDto model)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(model.RoleName));

            if (await checkIfRoleExists(model.RoleName)) { return BadRequest("this role is already existing"); }
            else
            {
                return Created();
            }
        }

        [HttpPost("Join-Role")]
        public async Task<IActionResult> JoinRole(string Role)
        {
            var user = await _userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _userManager.AddToRoleAsync(user, Role);

            if (result.Succeeded) { return Ok($@"{user.UserName} has become {Role}"); }
            else { return BadRequest(result.Errors); }
        }

        [HttpDelete("delete-Role")]
        public async Task<IActionResult> deleteRole(string roleName)
        {
            var role = await _roleManager.FindByIdAsync(roleName);
            var result = await _roleManager.DeleteAsync(role);

            if (result.Errors != null) {  return BadRequest(result.Errors); }
            return Ok(result);
        }
        #region Helper Functions
        private async Task<bool> checkIfRoleExists(string role)
        {
            return await _roleManager.Roles.AnyAsync(r => r.Name == role);
        }
        #endregion
    }
}
