using Backend.Data;

public class AuthenticationService
{
	private readonly BookAndDockContext _context;
    private readonly IConfiguration _configuration;

    public AuthenticationService(BookAndDockContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
}
