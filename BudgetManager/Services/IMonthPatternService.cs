using BudgetManager.Dto;
using BudgetManager.Dto.MonthPattern;
using BudgetManager.Models;

namespace BudgetManager.Services
{
    public interface IMonthPatternService
    {
        Task<MonthPatternDto> Get(int id);
        Task<IEnumerable<MonthPatternDto>> GetAll();
        Task<MonthPatternDto> Add(AddMonthPatternDto dto);
        Task Update(UpdateMonthPatternDto dto);
        Task Delete(int id);
        Task<Pattern> GetMonthPattern(MonthYearModel model);
    }
}