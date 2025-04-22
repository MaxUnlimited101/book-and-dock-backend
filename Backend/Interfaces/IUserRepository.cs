using Backend.Models;

namespace Backend.Interfaces;

public interface IUserRepository
{
    bool CheckIfUserExistsByIdAndRole(int id, Role role);

    bool CheckIfUserExistsById(int id);
    User? GetUserById(int id);
    List<User> GetAllUsers();
    void UpdateUser(User user);
    int CreateUser(User user);
    void DeleteUser(int id);
    User? GetUserByEmail(string email);
    User? GetUserByUsername(string username);
    List<User> GetUsersByRole(Role role);

    User? GetUserByPhoneNumber(string phoneNumber);
    Task<List<User>> GetAllUsersByIdAsync();
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> GetUserByPhoneNumberAsync(string phoneNumber);
    void UpdateUserAsync(User user);
    Task<int> CreateUserAsync(User user);
    Task DeleteUserAsync(int id);
    bool UpdateUserById(int id, User updatedUser);
}