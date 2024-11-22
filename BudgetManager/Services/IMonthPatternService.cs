using BudgetManager.Dto.MonthPattern;
using BudgetManager.Dto.Pattern;

namespace BudgetManager.Services
{
    public interface IMonthPatternService
    {
        Task<MonthPatternDto> RetrieveMonthPatternAsync(int id, Guid userId);
        Task<PatternDto> RetrieveMonthPatternAsync(int month, int year, Guid userId);
        Task<IEnumerable<MonthPatternDto>> RetrieveMonthPatternsAsync(Guid userId);
        Task<IEnumerable<FullMonthPatternDto>> RetrievePatternsAsync(Guid userId);
        Task<MonthPatternDto> AddMonthPatternAsync(AddMonthPatternDto dto);
        Task UpdateMonthPatternAsync(UpdateMonthPatternDto dto);
        Task DeleteMonthPatternAsync(int id, Guid userId);
    }
}