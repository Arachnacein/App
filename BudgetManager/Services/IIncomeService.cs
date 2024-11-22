using BudgetManager.Dto.Income;

namespace BudgetManager.Services
{
    public interface IIncomeService
    {
        Task<IEnumerable<IncomeDto>> RetrieveIncomesAsync(Guid userId);
        Task<IEnumerable<IncomeDto>> RetrieveIncomesAsync(int month, int year, Guid userId);
        Task<IncomeDto> RetrieveIncomeAsync(int id, Guid userId);
        Task<IncomeDto> AddIncomeAsync(AddIncomeDto income);
        Task UpdateIncomeAsync(UpdateIncomeDto income);
        Task DeleteIncomeAsync(int id, Guid userId);
    }
}