using Backend.DTO;
using Backend.Interfaces;

namespace Backend.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<IEnumerable<RoleDTO>> GetAllRolesAsync()
    {
        return await _roleRepository.GetAllRolesAsync();
    }

    public async Task<RoleDTO?> GetRoleByIdAsync(int id)
    {
        return await _roleRepository.GetRoleByIdAsync(id);
    }

    public async Task<int> CreateRoleAsync(RoleDTO role)
    {
        return await _roleRepository.CreateRoleAsync(role);
    }

    public async Task<bool> UpdateRoleAsync(RoleDTO role)
    {
        return await _roleRepository.UpdateRoleAsync(role);
    }

    public async Task DeleteRoleAsync(int id)
    {
        await _roleRepository.DeleteRoleAsync(id);
    }
}