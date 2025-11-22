using EFCore.NamingConventions;
using Microsoft.EntityFrameworkCore;
using ShkandalData.Models;
using ShkandalData.Repositories;
using ShkandalInfrastructure;
using ShkandalInfrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ShkandalDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("ShkandalConnection"),
        o => o.UseVector()).UseCamelCaseNamingConvention());

builder.Services.AddScoped<IClusterRepository, ClusterRepository>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
