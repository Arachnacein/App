using System;

namespace BudgetManager.Installers
{
    public static class InstallerExtensions
    {
        /// <summary>
        /// Installs services in the assemlby.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void InstallServiceInAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            //installers stores the installer classes created by our program
            var installers = typeof(Program).Assembly.ExportedTypes
                .Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IInstaller>()
                .ToList();
                                   
            //and register them
            installers
                .ForEach(installers => installers.InstallServices(services, configuration));                                                              //  i dokonał rejestracji serwisów poprzez wykonanie metod InstallSerwices z tych klas
        }

    }
}