using Backend.Models;

namespace Backend.Interfaces;

public interface IUserRepository
{
    bool CheckIfUserExistsByIdAndRole(int id, Role role);
}