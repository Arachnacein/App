using BudgetManager.Dto.Pattern;

namespace BudgetManager.Services
{
    public interface IPatternService
    {
        Task<IEnumerable<PatternDto>> RetrievePatterns(Guid userId);
        Task<PatternDto> RetrievePattern(int id, Guid userId);
        Task<PatternDto> AddPattern(AddPatternDto dto);
        Task DeletePattern(int id, Guid userId);
    }
}