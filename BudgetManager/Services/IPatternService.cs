using BudgetManager.Dto.Pattern;

namespace BudgetManager.Services
{
    public interface IPatternService
    {
        Task<IEnumerable<PatternDto>> RetrievePatterns();
        Task<PatternDto> RetrievePattern(int id);
        Task<PatternDto> AddPattern(AddPatternDto dto);
        Task DeletePattern(int id);
    }
}
