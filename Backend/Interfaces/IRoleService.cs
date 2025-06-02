using Backend.DTO;

namespace Backend.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<RoleDTO>> GetAllRolesAsync();
    Task<RoleDTO?> GetRoleByIdAsync(int id);
    Task<int> CreateRoleAsync(RoleDTO role);
    Task<bool> UpdateRoleAsync(RoleDTO role);
    Task DeleteRoleAsync(int id);
}