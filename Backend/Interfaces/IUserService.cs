using Backend.Models;

namespace Backend.Interfaces;

public interface IUserService
{
    bool CheckIfUserExistsByIdAndRole(int id, Role role);
}