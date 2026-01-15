using Microsoft.EntityFrameworkCore;
using ShkandalInfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;

namespace ShkanadalTests.RepositoryTests
{
    public class DatabaseFixture : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:15-alpine")
            .WithDatabase("test_db")
            .Build();

        public string ConnectionString => _dbContainer.GetConnectionString();

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            await InitializeDatabaseSchemaAsync();
        }
        private async Task InitializeDatabaseSchemaAsync()
        {
            var options = new DbContextOptionsBuilder<ShkandalDbContext>() 
                .UseNpgsql(ConnectionString)
                .UseCamelCaseNamingConvention() 
                .Options;

            using var context = new ShkandalDbContext(options);
            await context.Database.EnsureCreatedAsync();
            await context.Database.ExecuteSqlRawAsync("CREATE EXTENSION IF NOT EXISTS pg_trgm;");
        }
        public async Task DisposeAsync()
        {
            await _dbContainer.StopAsync();
        }

        public ShkandalDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ShkandalDbContext>()
                .UseNpgsql(ConnectionString)
                .UseCamelCaseNamingConvention()
                .Options;

            return new ShkandalDbContext(options);
        }
    }
}
