using Microsoft.EntityFrameworkCore;
using EFCore.NamingConventions;
using ShkandalInfrastructure;
using ShkandalData.Models;
using ShkandalData.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ShkandalDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("ShkandalConnection"),
        o => o.UseVector()).UseCamelCaseNamingConvention());

builder.Services.AddScoped<IUnitOfWork, IUnitOfWork>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
