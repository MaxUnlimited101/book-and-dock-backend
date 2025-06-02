using Backend.Data;
using Backend.DTO;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[Route("admin/users")]
[ApiController]
[Authorize] // 🔒 Protects all routes (requires JWT)
public class UsersController : ControllerBase
{
    private readonly BookAndDockContext _context;
    private readonly IUserService _userService;

    public UsersController(BookAndDockContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _context.Users.Include(u => u.Role).ToListAsync();

        var userDtos = users.Select(u => new UserDto
        {
            Id = u.Id,
            Name = u.Name,
            Surname = u.Surname,
            Email = u.Email,
            PhoneNumber = u.PhoneNumber,
            Role = u.Role?.Name ?? "User"
        });

        return Ok(userDtos);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto userDto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        user.Name = userDto.Name ?? user.Name;
        user.Surname = userDto.Surname ?? user.Surname;
        user.Email = userDto.Email ?? user.Email;
        user.PhoneNumber = userDto.PhoneNumber ?? user.PhoneNumber;
        user.Role = await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == userDto.Role) ?? user.Role;
        user.RoleId = user.Role?.Id ?? user.RoleId;

        await _context.SaveChangesAsync();
        return Ok(new { message = "User updated" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        try
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "User deleted" });
        }
        catch (DbUpdateException ex)
        {
            // This usually happens if the user has related records like Bookings, Reviews, etc.
            return StatusCode(500, new { error = "Cannot delete user because they are linked to other data (e.g., bookings, comments, etc.)." });
        }
    }

    /// <summary>
    /// Returns user counts by role, so (role names not matching) {"user": 213, "admin": 43, "dockOwner": 134}
    /// </summary>
    /// <returns></returns>
    [HttpGet("/UserCount")]
    public Dictionary<string, int> GetUserCountsByRoles()
    {
        Console.WriteLine("Getting user counts by roles");
        return _userService.CountUsersByRoles();
    }

}
