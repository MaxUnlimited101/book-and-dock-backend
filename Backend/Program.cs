using Backend.Data;
using Backend.Interfaces;
using Backend.Repositories;
using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Backend;

public class Program
{
    public static void Main(string[] args)
    {
        // Load environment variables
        DotNetEnv.Env.Load();

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddScoped<RegistrationService>();
        builder.Services.AddScoped<JwtService>();
        builder.Services.AddScoped<AuthenticationService>();

        // Add controllers
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        bool is_docker = Environment.GetEnvironmentVariable("IS_DOCKER_CONTAINER") == "TRUE";

        if (is_docker)
        {
            Console.WriteLine("Running in Docker container");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("------NOT WORKNING WITOUT MIGRATIONS------");
            }
            // Connect to PostgreSQL database in Docker container
            builder.Services.AddNpgsql<BookAndDockContext>(Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING"));
            builder.Services.AddDbContext<BookAndDockContext>(options =>
                options.UseNpgsql(Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING"))
            );
        }
        else
        {
            Console.WriteLine("Running in local environment");
            // Connect to local PostgreSQL database
            builder.Services.AddNpgsql<BookAndDockContext>(builder.Configuration.GetConnectionString("postgres"));
            builder.Services.AddDbContext<BookAndDockContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
            );
        }

        // Add context
        //builder.Services.AddNpgsql<BookAndDockContext>(builder.Configuration.GetConnectionString("postgres"));
        
        // Add repositories
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IDockingSpotRepository, DockingSpotRepository>();
        builder.Services.AddScoped<IGuideRepository, GuideRepository>();
        builder.Services.AddScoped<ICommentRepository, CommentRepository>();
        builder.Services.AddScoped<IBookingRepository, BookingRepository>();
        builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
        builder.Services.AddScoped<IPortRepository, PortRepository>();
        builder.Services.AddScoped<IGuideRepository, GuideRepository>();
        builder.Services.AddScoped<ILocationRepository, LocationRepository>();
        // builder.Services.AddScoped<IImageRepository, ImageRepository>();
        // builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
        // builder.Services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
        // builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        // builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
        
        // Add services
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IDockingSpotService, DockingSpotService>();
        builder.Services.AddScoped<IGuideService, GuideService>();
        builder.Services.AddScoped<ICommentService, CommentService>();
        builder.Services.AddScoped<IBookingService, BookingService>();
        builder.Services.AddScoped<IReviewService, ReviewService>();
        builder.Services.AddScoped<IGuideService, GuideService>();
        builder.Services.AddScoped<ILocationService, LocationService>();
        // builder.Services.AddScoped<IPortService, PortService>();
        // builder.Services.AddScoped<IImageService, ImageService>();
        // builder.Services.AddScoped<INotificationService, NotificationService>();
        // builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();
        // builder.Services.AddScoped<IRoleService, RoleService>();
        // builder.Services.AddScoped<IServiceService, ServiceService>();
        
        // Configure Npgsql to map DateTime to timestamp with time zone
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
        // builder.Services.AddDbContext<BookAndDockContext>(options =>
        //     options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        // );
        
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("🔴 Authentication failed:");
                        Console.WriteLine(context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("✅ Token validated:");
                        Console.WriteLine("User: " + context.Principal?.Identity?.Name);
                        return Task.CompletedTask;
                    }
                };

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
                    )
                };
            });


        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins("http://localhost:5173", "http://localhost:8080")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials(); 
            });
        });


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        // if (app.Environment.IsDevelopment())
        // {
        //     app.UseSwagger();
        //     app.UseSwaggerUI();
        // }
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseCors();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllers();

        app.Run();
    }
}