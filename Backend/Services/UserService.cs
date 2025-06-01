using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public bool CheckIfUserExistsByIdAndRole(int id, Role role)
    {
        return _userRepository.CheckIfUserExistsByIdAndRole(id, new Role());
    }

    public Task<List<User>> GetAllUsersByIdAsync()
    {
        return _userRepository.GetAllUsersByIdAsync();
    }
    public Task<User?> GetUserByIdAsync(int id)
    {
        return _userRepository.GetUserByIdAsync(id);
    }
    public Task<User?> GetUserByEmailAsync(string email)
    {
        return _userRepository.GetUserByEmailAsync(email);
    }
    public Task<User?> GetUserByUsernameAsync(string username)
    {
        return _userRepository.GetUserByUsernameAsync(username);
    }
    public Task<User?> GetUserByPhoneNumberAsync(string phoneNumber)
    {
        return _userRepository.GetUserByPhoneNumberAsync(phoneNumber);
    }

    public Task<bool> UpdateUserByIdAsync(int id, User updatedUser)
{
    return Task.FromResult(_userRepository.UpdateUserById(id, updatedUser));
}
    public Task DeleteUserByIdAsync(int id)
    {
        return _userRepository.DeleteUserAsync(id);
    }

    public Dictionary<string, int> CountUsersByRoles()
    {
        return _userRepository.CountUsersByRoles();
    }
}