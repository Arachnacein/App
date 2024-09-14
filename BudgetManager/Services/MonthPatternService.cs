using BudgetManager.Dto;
using BudgetManager.Dto.MonthPattern;
using BudgetManager.Exceptions.PatternExceptions;
using BudgetManager.Mappers;
using BudgetManager.Models;
using BudgetManager.Repositories;

namespace BudgetManager.Services
{
    public class MonthPatternService : IMonthPatternService
    {
        private readonly IMonthPatternRepository _monthPatternRepository;
        private readonly IPatternRepository _patternRepository;
        private readonly IMonthPatternMapper _mapper;

        public MonthPatternService(IMonthPatternRepository repository, IMonthPatternMapper mapper, IPatternRepository patternRepository)
        {
            _monthPatternRepository = repository;
            _mapper = mapper;
            _patternRepository = patternRepository;
        }

        public async Task<MonthPatternDto> Get(int id)
        {
            var monthPattern = await _monthPatternRepository.Get(id);
            if (monthPattern == null)
                throw new Exception($"Pattern not found exception. Id: {id}.");
            return _mapper.Map(monthPattern);
        }

        public async Task<IEnumerable<MonthPatternDto>> GetAll()
        {
            var monthPatterns = await _monthPatternRepository.GetAll();
            return _mapper.MapElements(monthPatterns.ToList());
        }

        public async Task<MonthPatternDto> Add(AddMonthPatternDto dto)
        {
            var checkPatternExists = _patternRepository.Get(dto.PatternId);
            if (checkPatternExists == null)
                throw new PatternNotFoundException($"Pattern not found. Id:{dto.PatternId}.");

            var mappedMonthPattern = _mapper.Map(dto);
            await _monthPatternRepository.Add(mappedMonthPattern);
            return _mapper.Map(mappedMonthPattern);
        }

        public async Task Update(UpdateMonthPatternDto dto)
        {
            var monthPattern = await _monthPatternRepository.Get(dto.Id);
            if (monthPattern == null)
                throw new Exception($"Pattern not found exception. Id: {dto.Id}.");
            var mappedMonthPattern = _mapper.Map(dto);
            await _monthPatternRepository.Update(mappedMonthPattern);
        }

        public async Task Delete(int id)
        {
            var monthPattern = await _monthPatternRepository.Get(id);
            if (monthPattern == null)
                throw new Exception($"Pattern not found exception. Id: {id}.");
            await _monthPatternRepository.Delete(monthPattern);
        }

        public async Task<Pattern> GetMonthPattern(MonthYearModel model)
        {
            var monthPattern = await _monthPatternRepository.Get(model);
            if (monthPattern == null)
                throw new PatternNotFoundException($"MonthPattern not found. Month: {model.Month}, year:{model.Year}.");

            var pattern = await _patternRepository.Get(monthPattern.PatternId);
            if(pattern == null)
                throw new PatternNotFoundException($"Pattern not found. Id:{pattern.Id}.");

            return pattern;
        }
    }
}