using Backend.Data;
using Backend.DTO;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly BookAndDockContext _context;

    public RoleRepository(BookAndDockContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RoleDTO>> GetAllRolesAsync()
    {
        return await _context.Roles
            .Select(r => new RoleDTO(r.Id, r.Name, r.CreatedOn!.Value))
            .ToListAsync();
    }

    public async Task<RoleDTO?> GetRoleByIdAsync(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        return role == null ? null : new RoleDTO(role.Id, role.Name, role.CreatedOn!.Value);
    }

    public async Task<int> CreateRoleAsync(RoleDTO role)
    {
        var newRole = new Role
        {
            Name = role.Name,
            CreatedOn = DateTime.UtcNow,
        };
        _context.Roles.Add(newRole);
        await _context.SaveChangesAsync();
        return newRole.Id;
    }

    public async Task<bool> UpdateRoleAsync(RoleDTO role)
    {
        var existingRole = await _context.Roles.FindAsync(role.Id);
        if (existingRole == null) return false;

        existingRole.Name = role.Name;
        _context.Roles.Update(existingRole);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task DeleteRoleAsync(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role != null)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }
    }
}