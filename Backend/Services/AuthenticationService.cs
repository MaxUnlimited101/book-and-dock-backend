using Backend.Data;
using Backend.Services;
using Microsoft.EntityFrameworkCore;


public class AuthenticationService
{
    private readonly BookAndDockContext _context;
    private readonly JwtService _jwtService;

    public AuthenticationService(BookAndDockContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<string> AuthenticateAsync(LoginRequestDto request)
    {
        var user = await _context.Users.Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            throw new Exception("Invalid email or password.");
        }

        return _jwtService.GenerateToken(user);
    }
}
