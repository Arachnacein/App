using BudgetManager.Dto;
using BudgetManager.Dto.MonthPattern;
using BudgetManager.Dto.Pattern;
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
        private readonly IMonthPatternMapper _monthPatternMapper;
        private readonly IPatternMapper _patternMapper;

        public MonthPatternService(IMonthPatternRepository repository, IMonthPatternMapper mapper, IPatternRepository patternRepository, IPatternMapper patternMapper)
        {
            _monthPatternRepository = repository;
            _monthPatternMapper = mapper;
            _patternRepository = patternRepository;
            _patternMapper = patternMapper;
        }

        public async Task<MonthPatternDto> RetrieveMonthPattern(int id)
        {
            var monthPattern = await _monthPatternRepository.Get(id);
            if (monthPattern == null)
                throw new Exception($"Pattern not found exception. Id: {id}.");
            return _monthPatternMapper.Map(monthPattern);
        }

        public async Task<IEnumerable<MonthPatternDto>> RetrieveMonthPatterns()
        {
            var monthPatterns = await _monthPatternRepository.GetAll();
            return _monthPatternMapper.MapElements(monthPatterns.ToList());
        }

        public async Task<MonthPatternDto> AddMonthPattern(AddMonthPatternDto dto)
        {
            var checkPatternExists = _patternRepository.Get(dto.PatternId);
            if (checkPatternExists == null)
                throw new PatternNotFoundException($"Pattern not found. Id:{dto.PatternId}.");

            var exists = await _monthPatternRepository.CheckExists(new MonthYearModel { Month =  dto.Date.Month, Year = dto.Date.Year});
            if (exists != 0)
                throw new MonthPatternAlreadyExistsException($"Pattern for Month:{dto.Date.Month} and Year:{dto.Date.Year} already exists.");

            var mappedMonthPattern = _monthPatternMapper.Map(dto);
            await _monthPatternRepository.Add(mappedMonthPattern);
            return _monthPatternMapper.Map(mappedMonthPattern);
        }

        public async Task UpdateMonthPattern(UpdateMonthPatternDto dto)
        {
            var monthPattern = await _monthPatternRepository.Get(dto.Id);
            if (monthPattern == null)
                throw new Exception($"Pattern not found exception. Id: {dto.Id}.");
            var mappedMonthPattern = _monthPatternMapper.Map(dto);
            await _monthPatternRepository.Update(mappedMonthPattern);
        }

        public async Task DeleteMonthPattern(int id)
        {
            var monthPattern = await _monthPatternRepository.Get(id);
            if (monthPattern == null)
                throw new Exception($"Pattern not found exception. Id: {id}.");
            await _monthPatternRepository.Delete(monthPattern);
        }

        public async Task<PatternDto> RetrieveMonthPattern(int month, int year)
        {
            var model = new MonthYearModel { Month = month, Year = year };
            var monthPattern = await _monthPatternRepository.Get(model);
            if (monthPattern == null)
                return new PatternDto { Id = -1 };

            var pattern = await _patternRepository.Get(monthPattern.PatternId);
            if(pattern == null)
                throw new PatternNotFoundException($"Pattern not found. Id:{pattern.Id}.");

            return _patternMapper.Map(pattern);
        }

        public async Task<IEnumerable<FullMonthPatternDto>> RetrievePatterns()
        {
            var monthpattern = await _monthPatternRepository.GetAll();
            var patterns = await _patternRepository.GetAll();

            var result = monthpattern.Join(patterns,
                                            monthpattern => monthpattern.Id,
                                            pattern => pattern.Id,
                                            (monthpattern, pattern) => new FullMonthPatternDto
                                            {
                                                Id = monthpattern.Id,
                                                Date = monthpattern.Date,
                                                Pattern = new PatternDto
                                                {
                                                    Id = pattern.Id,
                                                    Name = pattern.Name,
                                                    Value_Saves = pattern.Value_Saves,
                                                    Value_Fees = pattern.Value_Fees,
                                                    Value_Entertainment = pattern.Value_Entertainment
                                                }
                                            }
                                           );
            return result;
        }
    }
}