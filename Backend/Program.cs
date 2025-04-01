using Backend.Data;
using Backend.Interfaces;
using Backend.Repositories;
using Backend.Services;

namespace Backend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        
        // Add controllers
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Add context
        builder.Services.AddNpgsql<BookAndDockContext>(builder.Configuration.GetConnectionString("postgres"));
        
        // Add repositories
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IDockRepository, DockRepository>();
        builder.Services.AddScoped<IBookingRepository, BookingRepository>();
        
        // Add services
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IDockService, DockService>();
        builder.Services.AddScoped<IBookingService, BookingService>();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.MapControllers();

        app.Run();
    }
}