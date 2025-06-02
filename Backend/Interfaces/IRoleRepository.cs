using Backend.DTO;

namespace Backend.Interfaces;

public interface IRoleRepository
{
    Task<IEnumerable<RoleDTO>> GetAllRolesAsync();
    Task<RoleDTO?> GetRoleByIdAsync(int id);
    Task<int> CreateRoleAsync(RoleDTO role);
    Task<bool> UpdateRoleAsync(RoleDTO role);
    Task DeleteRoleAsync(int id);
}