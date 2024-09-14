using BudgetManager.Dto.MonthPattern;

namespace BudgetManager.Services
{
    public interface IMonthPatternService
    {
        Task<MonthPatternDto> Get(int id);
        Task<IEnumerable<MonthPatternDto>> GetAll();
        Task<MonthPatternDto> Add(AddMonthPatternDto dto);
        Task Update(UpdateMonthPatternDto dto);
        Task Delete(int id);
    }
}
