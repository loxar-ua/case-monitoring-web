using EFCore.NamingConventions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using shkandalData;
using ShkandalData.Models;
using ShkandalInfrastructure;
using ShkandalInfrastructure.Repositories;
using ShkandalServices;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddDbContext<ShkandalDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("ShkandalConnection"))
        .UseCamelCaseNamingConvention());

var jwtKey = builder.Configuration["Jwt:Key"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IClusterRepository, ClusterRepository>();
builder.Services.AddScoped<IAdminArticlesRepository, AdminArticlesRepository>();
builder.Services.AddScoped<IAdminClustersRepository, AdminClustersRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IClusterService, ClusterService>();
builder.Services.AddScoped<IAdminArticleService, AdminArticleService>();
builder.Services.AddScoped<IAdminClusterService, AdminClusterService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MapperProfile>();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowSpecificOrigins", policy =>
    {
        policy
            .WithOrigins("http://127.0.0.1:5500", "http://localhost:5500", "http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

app.UseRouting();
app.UseCors("MyAllowSpecificOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chatHub");

app.Run();
