using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SanalMarketAPI.Data;
using SanalMarketAPI.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add controllers
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Response Caching
builder.Services.AddResponseCaching();

// CORS configuration Geli�tirme a�amas�nda devre d��� b�rak�yorum
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowSpecificOrigin",
//        builder =>
//        {
//            builder.WithOrigins("http://localhost:44333", "http://localhost:7160") // UI projesinin �al��t��� port
//                   .AllowAnyMethod()                     // T�m HTTP metotlar�na izin ver
//                   .AllowAnyHeader()                     // Her t�rl� HTTP ba�l���na izin ver
//                   .AllowCredentials();                  // Kimlik do�rulama bilgilerini kabul et (JWT veya cookie)
//        });
//});

// Geli�tirme a�amas�nda t�m originlere izin verilecektir
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Add DbContext with SQL server connection
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

// Add Identity services for authentication
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// JWT settings from configuration
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

// JWT Authentication configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true
    };
});

// Add Authorization service
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add CORS middleware
app.UseCors("AllowAllOrigins"); //Sonraki a�amada "app.UseCors("AllowAllOrigins");" olarak yeniden d�zenlenecek


// Enable HTTPS redirection
app.UseHttpsRedirection();

// Add response caching middleware
app.UseResponseCaching();

// Enable authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Run the application
app.Run();
