using Microsoft.EntityFrameworkCore;
using EFCore.NamingConventions;
using ShkandalInfrastructure;
using shkandalData.Models;
using shkandal_api;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using shkandalData;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddControllers();

builder.Services.AddDbContext<ShkandalDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("ShkandalConnection"),
        o => o.UseVector()).UseCamelCaseNamingConvention());

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
