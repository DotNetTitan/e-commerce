using Ecommerce.Api.Middlewares.ExceptionHandler;
using Ecommerce.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Asp.Versioning;
using Ecommerce.Application.Interfaces;
using Ecommerce.Infrastructure.Services;
using Ecommerce.Application.Common.Models;
using Azure.Communication.Email;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;
using System.Threading.RateLimiting;
using MediatR;
using Ecommerce.Application.Common.Behaviors;
using System.Reflection;
using Ecommerce.Infrastructure.Persistence.Repositories;
using FluentValidation;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add configuration from environment variables
builder.Configuration.AddEnvironmentVariables();

// Determine the current environment
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

// Configure app settings based on the environment
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddUserSecrets<Program>(optional: true, reloadOnChange: true);

// Register AppSettings
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Add services to the container
ConfigureServices(builder.Services, builder.Configuration);

// Configure rate limiter
ConfigureRateLimiter(builder.Services, builder.Configuration);

builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

// Add exception handling middleware
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Add problem details
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline
ConfigurePipeline(app);

await app.RunAsync();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddMassTransit(x =>
    {
        x.SetKebabCaseEndpointNameFormatter();

        x.AddConsumers(Assembly.GetExecutingAssembly());
        x.AddConsumers(typeof(IRegisterAssembly).Assembly);

        x.UsingAzureServiceBus((context, cfg) =>
        {
            cfg.Host(configuration["AppSettings:AzureServiceBusConnectionString"]);

            cfg.ConfigureEndpoints(context);
        });
    });

    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(ConfigureSwagger);

    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(configuration["AppSettings:DefaultConnection"]));

    builder.Services.AddIdentity<IdentityUser, IdentityRole<string>>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

    services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(typeof(IRegisterAssembly).Assembly);
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    });

    services.AddValidatorsFromAssembly(typeof(IRegisterAssembly).Assembly);

    ConfigureAuthentication(services, configuration);
    ConfigureIdentityOptions(services);

    services.AddHttpContextAccessor();

    services.AddSingleton(new EmailClient(configuration["AppSettings:AzureCommunicationService"]));

    services.AddScoped<IAuthenticationService, AuthenticationService>();
    services.AddScoped<ICurrentUserService, CurrentUserService>();
    services.AddTransient<IEmailService, EmailService>();
    services.AddScoped<ITokenService, TokenService>();
    services.AddScoped<RefreshTokenService>();
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddScoped<IOrderRepository, OrderRepository>();
    services.AddScoped<IProductRepository, ProductRepository>();
    services.AddScoped<ICategoryRepository, CategoryRepository>();
    services.AddScoped<ICustomerRepository, CustomerRepository>();
    services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
    services.AddScoped<IReviewRepository, ReviewRepository>();
}

static void ConfigureRateLimiter(IServiceCollection services, IConfiguration configuration)
{
    var rateLimitWindowSeconds = configuration.GetValue<int>("AppSettings:RateLimiter:RateLimitWindowSeconds");
    var rateLimitWindow = TimeSpan.FromSeconds(rateLimitWindowSeconds);
    var retryAfter = $"{rateLimitWindow.TotalSeconds.ToString(NumberFormatInfo.InvariantInfo)} seconds";

    var permitLimit = configuration.GetValue<int>("AppSettings:RateLimiter:PermitLimit");

    services.AddRateLimiter(options =>
    {
        // (default policy)
        options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        {
            return RateLimitPartition.GetFixedWindowLimiter(httpContext.Request.Headers.UserAgent.ToString(), _ =>
                new FixedWindowRateLimiterOptions
                {
                    PermitLimit = permitLimit,
                    Window = rateLimitWindow
                }
            );
        });

        options.OnRejected = async (context, cancellationToken) =>
        {
            context.HttpContext.Response.Headers.RetryAfter = retryAfter;
            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context.HttpContext.Response.ContentType = "application/problem+json";

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status429TooManyRequests,
                Title = "Too Many Requests",
                Detail =
                    $"You have exceeded the allowed number of requests. Your current limit is {permitLimit} requests per {rateLimitWindowSeconds} seconds. Please try again later in {rateLimitWindowSeconds} seconds.",
                Instance = context.HttpContext.Request.Path,
                Type = RfcTypeUrls.TooManyRequests
            };

            var json = JsonSerializer.Serialize(problemDetails);

            await context.HttpContext.Response.WriteAsync(json, cancellationToken);
        };
    });
}

void ConfigureSwagger(SwaggerGenOptions options)
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "E-commerce API", Version = "v1" });

    // Add JWT Bearer token authentication
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your token in the text input below."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
}

void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
{
    var key = configuration["AppSettings:Jwt:Key"] ??
              throw new ArgumentNullException(nameof(configuration), "AppSettings:Jwt:Key is not configured");

    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["AppSettings:Jwt:Issuer"],
                ValidAudience = configuration["AppSettings:Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
            };
        });
}

void ConfigureIdentityOptions(IServiceCollection services)
{
    services.Configure<IdentityOptions>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.User.RequireUniqueEmail = true;
    });
}

void ConfigurePipeline(WebApplication webApplication)
{
    if (webApplication.Environment.IsDevelopment())
    {
        webApplication.UseSwagger();
        webApplication.UseSwaggerUI();
    }

    webApplication.UseHttpsRedirection();
    webApplication.UseAuthentication();
    webApplication.UseAuthorization();
    webApplication.MapControllers();

    webApplication.UseRateLimiter();
    webApplication.UseExceptionHandler();
}

public partial class Program
{
}