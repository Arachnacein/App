using BudgetManager.Repositories;
using BudgetManager.Services;

namespace BudgetManager.Installers
{
    public class ServiceInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ITransactionService, TransactionService>();

            services.AddScoped<IRecurringTransactionRepository, RecurringTransactionRepository>();
            services.AddScoped<IRecurringTransactionService, RecurringTransactionService>();

            services.AddScoped<IPatternRepository, PatternRepository>();
            services.AddScoped<IPatternService, PatternService>();

            services.AddScoped<IIncomeRepository, IncomeRepository>();
            services.AddScoped<IIncomeService, IncomeService>();

            services.AddScoped<IMonthPatternRepository, MonthPatternRepository>();
            services.AddScoped<IMonthPatternService, MonthPatternService>();
        }
    }
}