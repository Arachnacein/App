using BudgetManager.Dto;
using BudgetManager.Dto.MonthPattern;
using BudgetManager.Models;

namespace BudgetManager.Services
{
    public interface IMonthPatternService
    {
        Task<MonthPatternDto> RetrieveMonthPattern(int id);
        Task<IEnumerable<MonthPatternDto>> RetrieveMonthPatterns();
        Task<MonthPatternDto> AddMonthPattern(AddMonthPatternDto dto);
        Task UpdateMonthPattern(UpdateMonthPatternDto dto);
        Task DeleteMonthPattern(int id);
        Task<Pattern> RetrieveMonthPattern(MonthYearModel model);
    }
}