using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TransportManagementSystem.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Database Connection
builder.Services.AddDbContext<ApDb>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("db")));

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        // ❌ Your mistake:
        // builder.Configuration["JWT:Issure"]
        // "Issure" spelling is wrong

        // ✅ Correct spelling is "Issuer"
        ValidIssuer = builder.Configuration["JWT:Issuer"],

        ValidAudience = builder.Configuration["JWT:Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    };
});

// ❌ You forgot Authorization service
// Without this, authorization policies may not work properly

// ✅ Add Authorization
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure middleware pipeline

app.UseHttpsRedirection();

// ❌ If UseAuthentication() is missing,
// JWT token will never validate

// ✅ First authenticate user
app.UseAuthentication();

// ✅ Then check authorization
app.UseAuthorization();

app.MapControllers();

app.Run();