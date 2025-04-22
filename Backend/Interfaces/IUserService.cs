using Backend.Models;

namespace Backend.Interfaces;

public interface IUserService
{
    bool CheckIfUserExistsByIdAndRole(int id, Role role);

    Task<List<User>> GetAllUsersByIdAsync();

    Task<User> GetUserByIdAsync(int id);

    Task<User> GetUserByEmailAsync(string email);

    Task<bool> UpdateUserByIdAsync(int id, User updatedUser);
    Task DeleteUserByIdAsync(int id);    
}