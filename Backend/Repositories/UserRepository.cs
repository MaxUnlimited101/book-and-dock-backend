using Backend.Data;
using Backend.Interfaces;
using Backend.Models;

public class UserRepository : IUserRepository
{
    private readonly BookAndDockContext _bookAndDockContext;

    public UserRepository(BookAndDockContext bookAndDockContext)
    {
        _bookAndDockContext = bookAndDockContext;
    }

    public bool CheckIfUserExistsByIdAndRole(int id, Role role)
    {
        return _bookAndDockContext.Users.Any(u => u.Id == id && u.Role == role);
    }
}