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

    public bool CheckIfUserExistsById(int id)
    {
        return _bookAndDockContext.Users.Any(u => u.Id == id);
    }
    public User? GetUserById(int id)
    {
        return _bookAndDockContext.Users.Find(id);
    }
    public List<User> GetAllUsers()
    {
        return _bookAndDockContext.Users.ToList();
    }
    public void UpdateUser(User user)
    {
        _bookAndDockContext.Users.Update(user);
        _bookAndDockContext.SaveChanges();
    }
    public int CreateUser(User user)
    {
        int id = _bookAndDockContext.Users.Add(user).Entity.Id;
        _bookAndDockContext.SaveChanges();
        return id;
    }
    public void DeleteUser(int id)
    {
        _bookAndDockContext.Users.Remove(_bookAndDockContext.Users.Find(id)!);
        _bookAndDockContext.SaveChanges();
    }
    public User? GetUserByEmail(string email)
    {
        return _bookAndDockContext.Users.FirstOrDefault(u => u.Email == email);
    }
    public User? GetUserByUsername(string username)
    {
        return _bookAndDockContext.Users.FirstOrDefault(u => u.Name == username);
    }
    public List<User> GetUsersByRole(Role role)
    {
        return _bookAndDockContext.Users.Where(u => u.Role == role).ToList();
    }
}