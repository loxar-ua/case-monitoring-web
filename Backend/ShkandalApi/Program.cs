using EFCore.NamingConventions;
using Microsoft.EntityFrameworkCore;
using ShkandalData.Models;
using ShkandalInfrastructure;
using ShkandalInfrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ShkandalDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("ShkandalConnection"))
        .UseCamelCaseNamingConvention());

builder.Services.AddScoped<IClusterRepository, ClusterRepository>();
builder.Services.AddScoped<IAdminArticlesRepository, AdminArticlesRepository>();
builder.Services.AddScoped<IAdminClustersRepository, AdminClustersRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
