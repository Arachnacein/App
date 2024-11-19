using BudgetManager.Dto.MonthPattern;
using BudgetManager.Dto.Pattern;

namespace BudgetManager.Services
{
    public interface IMonthPatternService
    {
        Task<MonthPatternDto> RetrieveMonthPattern(int id, Guid userId);
        Task<IEnumerable<MonthPatternDto>> RetrieveMonthPatterns(Guid userId);
        Task<MonthPatternDto> AddMonthPattern(AddMonthPatternDto dto);
        Task UpdateMonthPattern(UpdateMonthPatternDto dto);
        Task DeleteMonthPattern(int id, Guid userId);
        Task<PatternDto> RetrieveMonthPattern(int month, int year, Guid userId);
        Task<IEnumerable<FullMonthPatternDto>> RetrievePatterns(Guid userId);
    }
}