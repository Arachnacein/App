using BudgetManager.Dto.Pattern;

namespace BudgetManager.Services
{
    public interface IPatternService
    {
        Task<IEnumerable<PatternDto>> RetrievePatternsAsync(Guid userId);
        Task<PatternDto> RetrievePatternAsync(int id, Guid userId);
        Task<PatternDto> AddPatternAsync(AddPatternDto dto);
        Task DeletePatternAsync(int id, Guid userId);
    }
}