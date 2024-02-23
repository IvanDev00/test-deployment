using Microsoft.EntityFrameworkCore;

namespace riskwatch.api.search.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }

    public static class GlobalDbConnection
    {
        public static string MysqlConnection { get; set; }
    }

    public static class DbConfiguration
    {
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            // Set the global database connection string
            GlobalDbConnection.MysqlConnection = configuration.GetConnectionString("MysqlConnection");
        }
    }
}
