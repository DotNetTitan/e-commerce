using Ecommerce.Api.Middlewares.ExceptionHandler;
using Ecommerce.Infrastructure.Settings;
using Ecommerce.Infrastructure.Tokens;
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
using Ecommerce.Application.Common;

var builder = WebApplication.CreateBuilder(args);

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
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(ConfigureSwagger);

    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(configuration["AppSettings:DefaultConnection"]));

    builder.Services.AddIdentity<IdentityUser, IdentityRole<string>>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IRegisterAssembly).Assembly));

    ConfigureAuthentication(services, configuration);
    ConfigureIdentityOptions(services);

    services.AddScoped<IAuthenticationService, AuthenticationService>();
    services.AddScoped<ITokenService, TokenService>();
    services.AddScoped<RefreshTokenService>();
    services.AddHttpContextAccessor();
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
    var key = configuration["AppSettings:Jwt:Key"] ?? throw new ArgumentNullException(nameof(configuration), "AppSettings:Jwt:Key is not configured");

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
}