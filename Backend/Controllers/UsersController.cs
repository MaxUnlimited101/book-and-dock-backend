using Backend.Data;
using Backend.DTOs;
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

    public UsersController(BookAndDockContext context)
    {
        _context = context;
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
    public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        user.Name = updatedUser.Name;
        user.Surname = updatedUser.Surname;
        user.Email = updatedUser.Email;
        user.PhoneNumber = updatedUser.PhoneNumber;
        user.RoleId = updatedUser.RoleId;

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

}
