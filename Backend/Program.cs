using Backend.Data;
using Backend.Interfaces;
using Backend.Repositories;
using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Npgsql;
using Amazon.S3;
using Amazon;

namespace Backend;

public class Program
{
    public static void Main(string[] args)
    {
        // Load environment variables
        DotNetEnv.Env.Load();

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
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

        // Swagger setup
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

        Stripe.StripeConfiguration.ApiKey = Environment.GetEnvironmentVariable("STRIPE__SECRET_KEY");

        // Get DB connection string from environment variable
        string connectionString = Environment.GetEnvironmentVariable("POSTGRES__DEFAULT_DB_CONNECTION_STRING")
            ?? throw new Exception("POSTGRES__DEFAULT_DB_CONNECTION_STRING not set");

        // Optional: create database if not exists
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using var command = new NpgsqlCommand(
                $"CREATE DATABASE \"{Environment.GetEnvironmentVariable("POSTGRES_DB")}\"", connection);
            try
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Database created successfully.");
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Database already exists or error: {ex.Message}");
            }
        }

        // Add DbContext
        builder.Services.AddDbContext<BookAndDockContext>(options =>
            options.UseNpgsql(Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING"))
        );

        // Register repositories
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IDockingSpotRepository, DockingSpotRepository>();
        builder.Services.AddScoped<IGuideRepository, GuideRepository>();
        builder.Services.AddScoped<ICommentRepository, CommentRepository>();
        builder.Services.AddScoped<IBookingRepository, BookingRepository>();
        builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
        builder.Services.AddScoped<IPortRepository, PortRepository>();
        builder.Services.AddScoped<ILocationRepository, LocationRepository>();
        builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
        builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
        builder.Services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();

        // Register services
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IDockingSpotService, DockingSpotService>();
        builder.Services.AddScoped<IGuideService, GuideService>();
        builder.Services.AddScoped<ICommentService, CommentService>();
        builder.Services.AddScoped<IBookingService, BookingService>();
        builder.Services.AddScoped<IReviewService, ReviewService>();
        builder.Services.AddScoped<ILocationService, LocationService>();
        builder.Services.AddScoped<IPortService, PortService>();
        builder.Services.AddScoped<INotificationService, NotificationService>();
        builder.Services.AddScoped<IServiceService, ServiceService>();
        builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();
        builder.Services.AddScoped<IRoleService, RoleService>();
        builder.Services.AddScoped<IImageService, ImageService>();

        // Amazon S3 Integration
        builder.Services.AddSingleton<IAmazonS3>(_ =>
        {
            return new AmazonS3Client(
                Environment.GetEnvironmentVariable("AWS__AccessKey")
                    ?? throw new Exception("AWS__AccessKey not set"),
                Environment.GetEnvironmentVariable("AWS__SecretKey")
                    ?? throw new Exception("AWS__SecretKey not set"),
                RegionEndpoint.GetBySystemName(
                    Environment.GetEnvironmentVariable("AWS__Region")
                    ?? throw new Exception("AWS__Region not set")
                )
            );
        });

        builder.Services.AddSingleton<S3Service>();

        // JWT Auth setup
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

        // CORS
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        // Stripe
        builder.Services.AddSingleton<StripeService>();

        // Build and configure app
        var app = builder.Build();

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
