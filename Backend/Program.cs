using Backend.Data;
using Backend.Interfaces;
using Backend.Repositories;
using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Npgsql;

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
            options.JsonSerializerOptions.ReferenceHandler =
                System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "BookAndDock API",
                Version = "v1"
            });

            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and then your valid JWT token."
            });

            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        // Ensure database exists
        // create if not exists
        string connectionString = Environment.GetEnvironmentVariable("POSTGRES__DEFAULT_DB_CONNECTION_STRING")
             ?? throw new Exception("POSTGRES_CONNECTION_STRING not set");

        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand($"CREATE DATABASE \"{Environment.GetEnvironmentVariable("POSTGRES_DB")}\"", connection))
            {
                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Database created successfully.");
                }
                catch (NpgsqlException ex)
                {
                    System.Console.WriteLine($"Error creating database: {ex.Message}, skipping...");
                }
            }
        }

        builder.Services.AddNpgsql<BookAndDockContext>(
            Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING"));
        builder.Services.AddDbContext<BookAndDockContext>(options =>
            options.UseNpgsql(Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING"))
        );

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


        // builder.Services.AddCors(options =>
        // {
        //     options.AddDefaultPolicy(policy =>
        //     {
        //         policy.WithOrigins("http://localhost:5173", "http://localhost:8080")
        //             .AllowAnyHeader()
        //             .AllowAnyMethod()
        //             .AllowCredentials();
        //     });
        // });


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