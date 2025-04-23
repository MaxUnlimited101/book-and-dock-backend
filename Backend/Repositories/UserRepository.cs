using Backend.Data;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

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

    public User? GetUserByPhoneNumber(string phoneNumber)
    {
        return _bookAndDockContext.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
    }
    public Task<List<User>> GetAllUsersByIdAsync()
    {
        return Task.FromResult(_bookAndDockContext.Users.ToList());
    }
    public Task<User?> GetUserByIdAsync(int id)
    {
        return Task.FromResult(_bookAndDockContext.Users.Find(id));
    }
    public Task<User?> GetUserByEmailAsync(string email)
    {
        return Task.FromResult(_bookAndDockContext.Users.FirstOrDefault(u => u.Email == email));
    }
    public Task<User?> GetUserByUsernameAsync(string username)
    {
        return Task.FromResult(_bookAndDockContext.Users.FirstOrDefault(u => u.Name == username));
    }
    public Task<User?> GetUserByPhoneNumberAsync(string phoneNumber)
    {
        return Task.FromResult(_bookAndDockContext.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber));
    }

    public void UpdateUserAsync(User user)
    {
        _bookAndDockContext.Users.Update(user);
        _bookAndDockContext.SaveChanges();
    }

    public Task<int> CreateUserAsync(User user)
    {
        int id = _bookAndDockContext.Users.Add(user).Entity.Id;
        _bookAndDockContext.SaveChanges();
        return Task.FromResult(id);
    }
    public Task DeleteUserAsync(int id)
    {
        _bookAndDockContext.Users.Remove(_bookAndDockContext.Users.Find(id)!);
        _bookAndDockContext.SaveChanges();
        return Task.CompletedTask;
    }

    public bool UpdateUserById(int id, User updatedUser)
    {
        var user = _bookAndDockContext.Users.Find(id);
        if (user == null)
        {
            return false;
        }

        user.Name = updatedUser.Name;
        user.Email = updatedUser.Email;
        user.PhoneNumber = updatedUser.PhoneNumber;
        user.Role = updatedUser.Role;

        _bookAndDockContext.SaveChanges();
        return true;
    }

    public Dictionary<string, int> CountUsersByRoles()
    {
        List<Role> roles = _bookAndDockContext.Roles.Include(role => role.Users).ToList();
        Dictionary<string, int> roleCount = new();
        foreach (var role in roles)
        {
            roleCount[role.Name] = role.Users.Count;
        }
        return roleCount;
    }

    public Task<bool> UpdateUserByIdAsync(int id, User updatedUser)
    {
        var user = _bookAndDockContext.Users.Find(id);
        if (user == null)
        {
            return Task.FromResult(false);
        }

        user.Name = updatedUser.Name;
        user.Email = updatedUser.Email;
        user.PhoneNumber = updatedUser.PhoneNumber;
        user.Role = updatedUser.Role;

        _bookAndDockContext.SaveChanges();
        return Task.FromResult(true);
    }

    
}