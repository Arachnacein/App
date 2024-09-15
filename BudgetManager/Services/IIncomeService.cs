using BudgetManager.Dto;
using BudgetManager.Dto.Income;

namespace BudgetManager.Services
{
    public interface IIncomeService
    {
        Task<IEnumerable<IncomeDto>> RetrieveIncomes();
        Task<IEnumerable<IncomeDto>> RetrieveIncomes(int month, int year);
        Task<IncomeDto> RetrieveIncome(int id);
        Task<IncomeDto> AddIncome(AddIncomeDto income);
        Task UpdateIncome(UpdateIncomeDto income);
        Task DeleteIncome(int id);
    }
}