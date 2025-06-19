using Microsoft.EntityFrameworkCore;
using BlogPortal.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BlogPortal.Repositories;
using BlogPortal.Services;
using BlogPortal.Filters;
using BlogPortal.Models;

var builder = WebApplication.CreateBuilder(args);

// DB Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// JWT Auth
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("Jwt");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Headers.TryGetValue("Authorization", out var authHeader) &&
                authHeader.ToString().StartsWith("Bearer "))
            {
                context.Token = authHeader.ToString().Substring("Bearer ".Length).Trim();
                Console.WriteLine("context.Token " + context.Token);
            }
            else if (context.Request.Cookies.TryGetValue("token", out var tokenFromCookie))
            {
                context.Token = tokenFromCookie;
            }

            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";

            var apiResponse = new BlogPortal.Shared.ApiResponse<object>(false, "No token provided or token is invalid.", null, new
            {
                timestamp = DateTime.UtcNow,
                path = context.Request.Path.Value
            });
            var result = System.Text.Json.JsonSerializer.Serialize(apiResponse);
            return context.Response.WriteAsync(result);
        }
    };
});

// Repositories & Services DI
builder.Services.AddAuthorization();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<BlogService>();
builder.Services.AddScoped<BlogRepository>();
builder.Services.AddScoped<MediaFileRepository>();
builder.Services.AddScoped<FileService>();

// Register Cloudinary settings
builder.Services.Configure<CloudinarySettings>(
builder.Configuration.GetSection("CloudinarySettings"));

// ✅ Global Exception Filter registered
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "BlogPortal API V1");
        options.RoutePrefix = "docs";
    });
}

app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == StatusCodes.Status404NotFound &&
        !context.Response.HasStarted)
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            success = false,
            message = "The requested resource was not found",
            statusCode = 404,
            timestamp = DateTime.UtcNow.ToString("o"),
            path = context.Request.Path
        };

        var json = System.Text.Json.JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
});


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => Results.Ok("🚀 Welcome to BlogPortal API! Use /api/v1/... for API access."));

app.Run();
