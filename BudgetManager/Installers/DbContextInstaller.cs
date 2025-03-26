
using BudgetManager.Data;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Installers
{
    public class DbContextInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            var db_host = Environment.GetEnvironmentVariable("db_host");
            var db_name = Environment.GetEnvironmentVariable("db_name");
            var db_password = Environment.GetEnvironmentVariable("db_password");
            var connString = $"Data Source={db_host};Initial Catalog={db_name};Persist Security Info=True;User ID=sa;Password={db_password};TrustServerCertificate=True;";


            services.AddDbContext<BudgetDbContext>(options =>
            {
                options.UseSqlServer(connString);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
        }
    }
}