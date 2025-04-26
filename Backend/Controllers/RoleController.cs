using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Services;
using Backend.Models;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoleController : ControllerBase
    {
        // private readonly IRoleService _roleService;

        // public RoleController(IRoleService roleService)
        // {
        //     _roleService = roleService;
        // }

        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<Role>>> GetAllRoles()
        // {
        //     var roles = await _roleService.GetAllRolesAsync();
        //     return Ok(roles);
        // }

        // [HttpGet("{id}")]
        // public async Task<ActionResult<Role>> GetRoleById(int id)
        // {
        //     var role = await _roleService.GetRoleByIdAsync(id);
        //     if (role == null)
        //     {
        //         return NotFound();
        //     }
        //     return Ok(role);
        // }

        // [HttpPost]
        // public async Task<ActionResult<Role>> CreateRole([FromBody] Role role)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return BadRequest(ModelState);
        //     }

        //     var createdRole = await _roleService.CreateRoleAsync(role);
        //     return CreatedAtAction(nameof(GetRoleById), new { id = createdRole.Id }, createdRole);
        // }

        // [HttpPut("{id}")]
        // public async Task<IActionResult> UpdateRole(int id, [FromBody] Role role)
        // {
        //     if (id != role.Id || !ModelState.IsValid)
        //     {
        //         return BadRequest();
        //     }

        //     var updated = await _roleService.UpdateRoleAsync(role);
        //     if (!updated)
        //     {
        //         return NotFound();
        //     }

        //     return NoContent();
        // }

        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteRole(int id)
        // {
        //     var deleted = await _roleService.DeleteRoleAsync(id);
        //     if (!deleted)
        //     {
        //         return NotFound();
        //     }

        //     return NoContent();
        // }
    }
}