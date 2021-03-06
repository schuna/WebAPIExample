using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace WebApi.Persistence
{

    public class WebApiDbContextFactory : IDesignTimeDbContextFactory<WebApiDbContext>
    {
        public WebApiDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<WebApiDbContext>();
            var connectionString = configuration.GetConnectionString("WebApiDatabase");

            builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new WebApiDbContext(builder.Options);
        }
    }
}