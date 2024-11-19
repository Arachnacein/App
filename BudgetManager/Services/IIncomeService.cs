using BudgetManager.Dto;
using BudgetManager.Dto.Income;

namespace BudgetManager.Services
{
    public interface IIncomeService
    {
        Task<IEnumerable<IncomeDto>> RetrieveIncomes(Guid userId);
        Task<IEnumerable<IncomeDto>> RetrieveIncomes(int month, int year, Guid userId);
        Task<IncomeDto> RetrieveIncome(int id, Guid userId);
        Task<IncomeDto> AddIncome(AddIncomeDto income);
        Task UpdateIncome(UpdateIncomeDto income);
        Task DeleteIncome(int id, Guid userId);
    }
}