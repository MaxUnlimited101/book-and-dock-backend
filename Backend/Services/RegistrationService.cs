using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

public class RegistrationService
{
    private readonly BookAndDockContext _context;

    public RegistrationService(BookAndDockContext context)
    {
        _context = context;
    }

    public async Task<User> RegisterAsync(RegisterRequestDto registerRequest)
    {
        if (await _context.Users.AnyAsync(u => u.Email == registerRequest.Email))
            throw new Exception("Email is already registered.");

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);

        var user = new User
        {
            Name = registerRequest.Name,
            Surname = registerRequest.Surname,
            Email = registerRequest.Email,
            Password = hashedPassword,
            RoleId = 1,
            CreatedOn = DateTime.UtcNow
        };
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

        return user;
    }
}
