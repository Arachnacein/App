using BudgetManager.Mappers;

namespace BudgetManager.Installers
{
    public class MapperInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped<ITransactionMapper, TransactionMapper>();
            services.AddScoped<IPatternMapper, PatternMapper>();
            services.AddScoped<IIncomeMapper, IncomeMapper>();
            services.AddScoped<IMonthPatternMapper, MonthPatternMapper>();
            services.AddScoped<IRecurringTransactionMapper, RecurringTransactionMapper>();
        }
    }
}