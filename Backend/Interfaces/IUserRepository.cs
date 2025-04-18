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
}