using BudgetManager.Dto.MonthPattern;
using BudgetManager.Dto.Pattern;

namespace BudgetManager.Services
{
    public interface IMonthPatternService
    {
        Task<MonthPatternDto> RetrieveMonthPattern(int id);
        Task<IEnumerable<MonthPatternDto>> RetrieveMonthPatterns();
        Task<MonthPatternDto> AddMonthPattern(AddMonthPatternDto dto);
        Task UpdateMonthPattern(UpdateMonthPatternDto dto);
        Task DeleteMonthPattern(int id);
        Task<PatternDto> RetrieveMonthPattern(int month, int year);
        Task<IEnumerable<FullMonthPatternDto>> RetrievePatterns();
    }
}