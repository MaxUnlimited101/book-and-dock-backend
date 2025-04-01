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
}