namespace BudgetManager.Installers
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration Configuration);
    }
}