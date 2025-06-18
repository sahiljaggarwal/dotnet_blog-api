using Microsoft.EntityFrameworkCore;
using BlogPortal.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BlogPortal.Repositories;
using BlogPortal.Services;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!)),
        // RoleClaimType = "role"
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
{
    // Authorization header
    if (context.Request.Headers.TryGetValue("Authorization", out var authHeader) &&
        authHeader.ToString().StartsWith("Bearer "))
    {
        context.Token = authHeader.ToString().Substring("Bearer ".Length).Trim();
        Console.WriteLine("context.Token " + context.Token);
    }

    // Cookie
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

builder.Services.AddAuthorization();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<BlogService>();
builder.Services.AddScoped<BlogRepository>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();


var app = builder.Build();
if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "BlogPortal API V1");
        options.RoutePrefix = "docs";
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.MapGet("/", () => Results.Ok("🚀 Welcome to BlogPortal API! Use /api/v1/... for API access."));
app.Run();